using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PaymentHub.Configuration;
using PaymentHub.Network.Extensions;
using PaymentHub.Network.Interfaces;
using PaymentHub.Network.Responses;

namespace PaymentHub.Network.Services
{
    public class PayPalAuthService : IPayPalAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Logger<PayPalAuthService> _logger;
        private readonly PayPalSettings _settings;

        public PayPalAuthService(IHttpClientFactory httpClientFactory, Logger<PayPalAuthService> logger,PayPalSettings settings)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
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
            { 
                var failureReason = HttpStatusFailureMap.Resolve(response.StatusCode);

                _logger.LogWarning(
                    "HTTP request failed with {StatusCode} mapped to {FailureReason}",
                    response.StatusCode,
                    failureReason);
            }


            var raw = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<PayPalTokenResponse>(raw);
            var token = tokenResponse?.AccessToken;

            if (tokenResponse is null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                _logger.LogError("Failed to obtain PayPal access token.");

            return token;
        }
    }
}
