using Microsoft.Extensions.Configuration;
using PaymentHub.Application.Interfaces;
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
        private readonly IConfiguration _config;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _baseUrl;
        private readonly string _currency;

        public PayPalService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;


            // bring using statements for Microsoft.Extensions.Configuration
            _clientId = config["PayPal:ClientId"]!;
            _secret = config["PayPal:Secret"]!;
            _baseUrl = config["PayPal:SandboxBaseUrl"]!;
            _currency = config["PayPal:Currency"]!;
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
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_secret}"));
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
