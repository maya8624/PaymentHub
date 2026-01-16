using PaymentHub.Application.Interfaces;
using PaymentHub.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PaymentHub.Configuration;
using PaymentHub.Application.Extensions;

namespace PaymentHub.Application.Services
{
    public class PayPalAuthService : IPayPalAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PayPalSettings _settings;

        public PayPalAuthService(IHttpClientFactory httpClientFactory, PayPalSettings settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings;
        }

        public async Task<string> GetAccessToken()
        {
            var tokenUrl = _settings.SandboxBaseUrl.CombineUrl(_settings.SandboxTokenUrl);
            var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);

            var byteArray = Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.Secret}");
            var auth = Convert.ToBase64String(byteArray);

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            });

            using var http = _httpClientFactory.CreateClient();
            var response = await http.SendAsync(request);

            if (response.IsSuccessStatusCode == false)
                return null;

            var raw = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<PayPalTokenResponse>(raw);
            var token = tokenResponse?.AccessToken;

            if (tokenResponse is null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {

            }

            return token;
        }
    }
}
