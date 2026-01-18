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
using System.Text.Json;
using PaymentHub.Infrastructure.Interfaces;
using PaymentHub.Application.Extensions;

namespace PaymentHub.Application.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IPayPalAuthService _authService;
        private readonly IHttpRequestSender _httpRequestSender;
        private readonly ILogger<PayPalAuthService> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly PayPalSettings _settings;
        private readonly IUnitOfWork _uow;

        public PayPalService(
            IPayPalAuthService authService,
            ILogger<PayPalAuthService> logger,
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IOptions<PayPalSettings> settings,
            IUnitOfWork uow,
            IHttpRequestSender httpRequestSender)
        {
            _authService = authService;
            _logger = logger;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _settings = settings.Value;
            _uow = uow;
            _httpRequestSender = httpRequestSender;
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

            var accessToken = await _authService.GetAccessToken();

            var request = BuildCaptureRequest(payment.ProviderOrderId, accessToken);

            var response = await _httpRequestSender.ExecuteRequest<PayPalCaptureResponse>(request);//, ct);

            await ApplyCaptureResult(payment, response);

            return response;
        }

        //TODO: split into small methods
        private async Task ApplyCaptureResult(Payment payment, PayPalCaptureResponse response)
        {
            var capture = response
                ?.PurchaseUnits.FirstOrDefault()
                ?.PaymentDetails
                ?.Captures.FirstOrDefault();

            if (capture == null)
                throw new NotFoundException(ApplicationErrorCodes.NotFound, "capture is not found");

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

        private async Task<PayPalCaptureResponse> CaptureWithPayPal(string paypalOrderId)// CancellationToken ct)
        {
            var accessToken = await _authService.GetAccessToken();

            var request = BuildCaptureRequest(paypalOrderId, accessToken);
            
            var response = await _httpRequestSender.ExecuteRequest<PayPalCaptureResponse>(request);//, ct);


            //var json = await response.Content.ReadAsStringAsync();
            //var result = json.SafeDeserialize<PayPalCaptureResponse>();
            return response;
        }

        //private static async Task<HttpResponseMessage> SendWithRetry(
        //    HttpClient client,
        //    HttpRequestMessage request,
        //    CancellationToken ct)
        //{
        //    const int maxRetries = 3;

        //    for (var attempt = 1; attempt <= maxRetries; attempt++)
        //    {
        //        var response = await client.SendAsync(request, ct);

        //        if (response.IsSuccessStatusCode)
        //            return response;

        //        if (attempt == maxRetries)
        //            response.EnsureSuccessStatusCode();

        //        await Task.Delay(TimeSpan.FromSeconds(attempt), ct);
        //    }

        //    throw new InvalidOperationException("Retry loop exited unexpectedly");
        //}

        #region private methods
        private HttpRequestMessage BuildCaptureRequest(string paypalOrderId, string accessToken)
        {
            var captureUrl = string.Format(_settings.SandboxCaptureOrderUrl, paypalOrderId);
            var url = _settings.SandboxBaseUrl.CombineUrl(captureUrl);
            var idempotencyKey = Guid.NewGuid().ToString();

            var request = HttpRequestFactory.CreateJson(
                HttpMethod.Post,
                url: url,
                bearerToken: accessToken,
                headers: new Dictionary<string, string>
                {
                    ["PayPal-Request-Id"] = idempotencyKey
                });

            return request;
        }

        private async Task<string> CreatePayPalOrder(OrderForPaymentResponse order, string accessToken, string idempotencyKey)
        {
            try
            {
                var body = BuildPayPalOrderRequest(order);
                var url = _settings.SandboxBaseUrl.CombineUrl(_settings.SandboxCreateOrderUrl);
                var request = HttpRequestFactory.CreateJson(
                  HttpMethod.Post,
                  url,
                  body,
                  accessToken,
                  headers: new Dictionary<string, string>
                  {
                      ["PayPal-Request-Id"] = idempotencyKey
                  });

                var result = await _httpRequestSender.ExecuteRequest<PayPalOrderResponse>(request);

                if (result == null)
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
