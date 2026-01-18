using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentHub.Configuration;
using PaymentHub.Network.Enums;
using PaymentHub.Network.Exceptions;
using PaymentHub.Network.Extensions;
using PaymentHub.Network.Interfaces;
using PaymentHub.Network.Responses;

namespace PaymentHub.Network.Services
{
    public class PayPalAuthService : IPayPalAuthService
    {
        private readonly IHttpRequestSender _httpRequestSender;
        private readonly ILogger<PayPalAuthService> _logger;
        private readonly PayPalSettings _settings;

        public PayPalAuthService(IHttpRequestSender httpRequestSender, ILogger<PayPalAuthService> logger, IOptions<PayPalSettings> settings)
        {
            _httpRequestSender = httpRequestSender;
            _logger = logger;
            _settings = settings.Value;
        }

        //TODO: cache a token and reuse it until expires?
        public async Task<string> GetAccessToken()
        {            
            var options = BuildTokenRequest();
            var request = HttpRequestFactory.CreateJson(options);

            var response = await _httpRequestSender.ExecuteRequest<PayPalTokenResponse>(request);
            var token = response?.AccessToken;

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError("Failed to obtain PayPal access token.");

                throw new PayPalException(
                    PayPalErrorCodes.PayPalAccessTokenFailed,
                    "PayPal token response was invalid or missing access token.");
            }
            
            return token;
        }

        private RequestBuilderOptions BuildTokenRequest()
        {
            var url = _settings.SandboxBaseUrl.CombineUrl(_settings.SandboxTokenUrl);
            var idempotencyKey = Guid.NewGuid().ToString();

            var byteArray = Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.Secret}");
            var auth = Convert.ToBase64String(byteArray);
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            });

            var options = new RequestBuilderOptions
            {
                AuthScheme = AuthScheme.Basic,
                AuthToken = auth,
                Method = HttpMethod.Post,
                Content = content,
                Url = url,
            };

            return options;
        }
    }
}
