using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentHub.Application.Dtos;
using PaymentHub.Application.Enums;
using PaymentHub.Application.Exceptions;
using PaymentHub.Application.Interfaces;
using PaymentHub.Configuration;
using PaymentHub.Domain.Enums;
using PaymentHub.Infrastructure.Responses;
using PaymentHub.Network;
using PaymentHub.Network.Enums;
using PaymentHub.Network.Exceptions;
using PaymentHub.Network.Extensions;
using PaymentHub.Network.Interfaces;
using PaymentHub.Network.Services;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Domain.Enums;
using PayPalIntegration.Infrastructure.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using PaymentHub.Infrastructure.Interfaces;
using PaymentHub.Infrastructure.Repositories;
using System.Threading;
using PaymentHub.Application.Extensions;

namespace PaymentHub.Application.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IPayPalAuthService _authService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PayPalAuthService> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly PayPalSettings _settings;
        private readonly IUnitOfWork _uow;

        public PayPalService(
            IPayPalAuthService authService,
            IHttpClientFactory httpClientFactory,
            ILogger<PayPalAuthService> logger,
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IOptions<PayPalSettings> settings,
            IUnitOfWork uow)
        {
            _authService = authService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _settings = settings.Value;
            _uow = uow;
        }

        public async Task<string> CreateOrder(int orderId)
        {   
            var order = await _orderRepository.GetOrderForPayment(orderId);

            if (order == null)
                throw new NotFoundException(ApplicationErrorCodes.NotFound, $"Order: {orderId} not found.");

            var backendIdempotencyKey = Guid.NewGuid().ToString();

            var payment = await SavePaymentRecord(order, backendIdempotencyKey);

            var accessToken = await _authService.GetAccessToken();

            var paypalOrderId = await CreatePayPalOrder(order, accessToken, backendIdempotencyKey);

            await UpdatePayment(payment, paypalOrderId);

            return paypalOrderId;
        }

        public async Task<PayPalCaptureResponse> CaptureOrder(int orderId)//, string providerOrderId)
        {
            var payment = await _paymentRepository.GetByOrderId(orderId);

            if (payment == null)
                throw new NotFoundException(ApplicationErrorCodes.NotFound, $"Payment not found. orderId:{orderId}");

            if (payment.Status == PaymentStatus.Completed && string.IsNullOrEmpty(payment.RawResponse) == false)
                return payment.RawResponse.SafeDeserialize<PayPalCaptureResponse>();

            var idempotencyKey = Guid.NewGuid().ToString();

            var response = await CaptureWithPayPal(payment.ProviderOrderId, idempotencyKey);//, ct);

            // 4. Persist result to THIS payment only
            await ApplyCaptureResult(payment, response);

            return response;
        }

        private async Task ApplyCaptureResult(Payment payment, PayPalCaptureResponse response)
        {
            var capture = response
                .PurchaseUnits?.FirstOrDefault()
                .PaymentDetails?.Captures?.FirstOrDefault();

            if (capture == null)
                throw new Exception("capture is not found");

            payment.Status = capture.Status switch
            {
                "COMPLETED" => PaymentStatus.Completed,
                "PENDING" => PaymentStatus.Pending,
                _ => PaymentStatus.Failed
            };

            payment.ProviderCaptureId = capture.Id;
            payment.CapturedAmount = decimal.Parse(capture.Amount.Value);
            payment.CapturedAt = DateTimeOffset.UtcNow;
            payment.RawResponse = JsonSerializer.Serialize(response);

            _paymentRepository.Update(payment);
            await _uow.SaveChanges();
        }


        private async Task<PayPalCaptureResponse> CaptureWithPayPal(string paypalOrderId, string idempotencyKey)// CancellationToken ct)
        {
            var accessToken = await _authService.GetAccessToken();

            var request = BuildCaptureRequest(paypalOrderId, accessToken, idempotencyKey);

            var client = _httpClientFactory.CreateClient();

            //var response = await SendWithRetry(client, request, ct);
            var response = await client.SendAsync(request);//, ct);

            if (response.IsSuccessStatusCode == false)
            {
                var failureReason = HttpStatusFailureMap.Resolve(response.StatusCode);

                _logger.LogWarning(
                    "HTTP request during CaptureWithPayPal failed with {StatusCode} mapped to {FailureReason}",
                    response.StatusCode,
                    failureReason
                );
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = json.SafeDeserialize<PayPalCaptureResponse>();
            return result;
        }

        private static async Task<HttpResponseMessage> SendWithRetry(
            HttpClient client,
            HttpRequestMessage request,
            CancellationToken ct)
        {
            const int maxRetries = 3;

            for (var attempt = 1; attempt <= maxRetries; attempt++)
            {
                var response = await client.SendAsync(request, ct);

                if (response.IsSuccessStatusCode)
                    return response;

                if (attempt == maxRetries)
                    response.EnsureSuccessStatusCode();

                await Task.Delay(TimeSpan.FromSeconds(attempt), ct);
            }

            throw new InvalidOperationException("Retry loop exited unexpectedly");
        }

        #region private methods
        private HttpRequestMessage BuildCaptureRequest(string paypalOrderId, string accessToken, string idempotencyKey)
        {
            var captureUrl = string.Format(_settings.SandboxCaptureOrderUrl, paypalOrderId);
            var url = _settings.SandboxBaseUrl.CombineUrl(captureUrl);
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("PayPal-Request-Id", idempotencyKey);
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

            return request;
        }

        private async Task<string> CreatePayPalOrder(OrderForPaymentResponse order, string accessToken, string idempotencyKey)
        {
            try
            { 
                var requestBody = BuildPayPalOrderRequest(order);
                var url = _settings.SandboxBaseUrl.CombineUrl(_settings.SandboxCreateOrderUrl);

                var request = CreateHttpRequestMessage(
                    HttpMethod.Post,
                    url,
                    requestBody,
                    accessToken,
                    idempotencyKey
                );

                var client = _httpClientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode == false)
                {
                    var failureReason = HttpStatusFailureMap.Resolve(response.StatusCode);

                    _logger.LogWarning(
                        "HTTP request failed with {StatusCode} mapped to {FailureReason}",
                        response.StatusCode,
                        failureReason
                    );
                }

                var result = await response.Content.ReadFromJsonAsync<PayPalOrderResponse>();

                if (result == null || string.IsNullOrEmpty(result.Id))
                {
                    _logger.LogError("Failed to create PayPal order.");
                    throw new PayPalAuthenticationException(PayPalErrorCodes.PayPalCreateOrderFailed, "Failed to create PayPal order.");
                }

                return result.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create PayPal order for Payment {OrderId}", order.Id);
                throw; 
                //TODO:create a custom exception
                // new PayPalOrderCreationException(payment.Id, "Could not create PayPal order. Please try again.");
            }
        }

        private HttpRequestMessage CreateHttpRequestMessage(
            HttpMethod method,
            string url,
            object? body = null,
            string? bearerToken = null,
            string? idempotencyKey = null)
        {
            {
                var request = new HttpRequestMessage(method, url);

                if (body != null)
                    request.Content = JsonContent.Create(body);

                if (string.IsNullOrEmpty(bearerToken) == false)
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                if (string.IsNullOrEmpty(idempotencyKey) == false)
                    request.Headers.Add("PayPal-Request-Id", idempotencyKey); // PayPal idempotency header

                return request;
            }
        }

        private object BuildPayPalOrderRequest(OrderForPaymentResponse order)
        {
            return new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        reference_id = order.Id.ToString(),
                        amount = new
                        {
                            currency_code = order.Currency.ToString(),
                            value = order.TotalAmount.ToString("F2")
                        },
                        items = order.Items.Select(i => new
                        {
                            name = i.ProductName,
                            unit_amount = new
                            {
                                currency_code = order.Currency.ToString(),
                                value = i.UnitPrice.ToString("F2")
                            },
                            quantity = i.Quantity.ToString()
                        }).ToArray()
                    }
                }
            };
        }

        private async Task<Payment> SavePaymentRecord(OrderForPaymentResponse order, string backendIdempotencyKey)
        {
            try
            {
                var payment = new Payment
                {
                    OrderId = order.Id,
                    Provider = PaymentProvider.PayPal,
                    Status = PaymentStatus.Pending,
                    Amount = order.TotalAmount,
                    Currency = order.Currency,
                    BackendIdempotencyKey = backendIdempotencyKey,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };

                await _paymentRepository.Create(payment);
                await _uow.SaveChanges();

                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save Payment record for Order {OrderId}", order.Id);
                throw new PaymentSaveException(ApplicationErrorCodes.PaymentSaveFailed, "Could not save payment record. Please try again.");
            }
        }

        private async Task UpdatePayment(Payment payment, string paypalOrderId)
        {
            await _uow.SaveChanges();
            try
            {
                payment.ProviderOrderId = paypalOrderId;
                payment.UpdatedAt = DateTimeOffset.UtcNow;

                _paymentRepository.Update(payment);
                await _uow.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update Payment record for Order {OrderId}", payment.OrderId);
                throw new PaymentSaveException(ApplicationErrorCodes.PaymentSaveFailed, "Failed to process payment. Please try again.");
            }
        }

        #endregion
    }
}
