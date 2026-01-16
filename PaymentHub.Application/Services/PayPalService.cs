using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PaymentHub.Application.Extensions;
using PaymentHub.Application.Interfaces;
using PaymentHub.Application.Responses;
using PaymentHub.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PaymentHub.Application.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPayPalAuthService _authService;
        private readonly PayPalSettings _settings;

        public PayPalService(IHttpClientFactory httpClientFactory, IOptions<PayPalSettings> settings, IPayPalAuthService authService)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
            _authService = authService;
        }

        public async Task<string> CreateOrder(decimal amount, string currencyCode)
        {
            var token = await _authService.GetAccessToken();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api-m.sandbox.paypal.com/v2/checkout/orders");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                new { amount = new { currency_code = currencyCode, value = amount.ToString("F2") } }
            }
            });

            using var http = _httpClientFactory.CreateClient();
            var response = await http.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadFromJsonAsync<JsonDocument>();
            return json!.RootElement.GetProperty("id").GetString()!;
        }
                      

        //private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string url, string token, string? idempotencyKey = null, object? body = null)
        //{
        //    var request = new HttpRequestMessage(method, url);
        //    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //    if (!string.IsNullOrEmpty(idempotencyKey))
        //        request.Headers.Add("PayPal-Request-Id", idempotencyKey);

        //    if (body != null)
        //        request.Content = JsonContent.Create(body);

        //    return request;
        //}
    }
}
