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

namespace PaymentHub.Application.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IPayPalAuthService _authService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PayPalAuthService> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentService _paymentService;
        private readonly PayPalSettings _settings;

        public PayPalService(
            IPayPalAuthService authService,
            IHttpClientFactory httpClientFactory,
            ILogger<PayPalAuthService> logger,
            IOrderRepository orderRepository,
            IPaymentService paymentService,
            IOptions<PayPalSettings> settings)
        {
            _authService = authService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _orderRepository = orderRepository;
            _paymentService = paymentService;
            _settings = settings.Value;
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

            await _paymentService.SavePayment(payment);

            return payment;
        }

        private async Task UpdatePayment(Payment payment, string paypalOrderId)
        {
            payment.ProviderOrderId = paypalOrderId;
            payment.UpdatedAt = DateTimeOffset.UtcNow;

            await _paymentService.UpdatePayment(payment);
        }
    }
}
