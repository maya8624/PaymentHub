using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PaymentHub.Application.Interfaces;
using PaymentHub.Configuration;
using PayPalIntegration.Domain.Enums;
using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace PaymentHub.Application.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PayPalSettings _settings;

        public PayPalService(IHttpClientFactory httpClientFactory, IOptions<PayPalSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public async Task<string> CreateOrder(decimal amount, string currencyCode)
        {
            var token = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api-m.sandbox.paypal.com/v2/checkout/orders");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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

        private async Task<string> GetAccessTokenAsync()
        {
            var byteArray = Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.Secret}");
            var auth = Convert.ToBase64String(byteArray);

            var url = new Uri(new Uri(_settings.SandboxBaseUrl), _settings.SandboxTokenUrl);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api-m.sandbox.paypal.com/v1/oauth2/token");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });

            using var http = _httpClientFactory.CreateClient();
            var response = await http.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadFromJsonAsync<JsonDocument>();

            return json!.RootElement.GetProperty("access_token").GetString()!;
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
