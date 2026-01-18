using Microsoft.Extensions.Logging;
using PaymentHub.Network.Exceptions;
using PaymentHub.Network.Interfaces;
using System.Net.Http.Json;

namespace PaymentHub.Network.Services
{
    public class HttpRequestSender : IHttpRequestSender
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<PayPalAuthService> _logger;

        public HttpRequestSender(IHttpClientFactory httpClientFactory, ILogger<PayPalAuthService> logger)
        {
            _httpClient = httpClientFactory;
            _logger = logger;
        }

        public async Task<T> ExecuteRequest<T>(HttpRequestMessage request)//, CancellationToken? cancellationToken)
        {
            try
            {
                using var http = _httpClient.CreateClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode == false)
                {
                    //var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    var failureReason = HttpStatusFailureMap.Resolve(response.StatusCode);

                    _logger.LogWarning(
                        "HTTP request failed with {StatusCode} mapped to {FailureReason}",
                        response.StatusCode,
                        failureReason);
                }

                var content = await response.Content.ReadFromJsonAsync<T>();
                if (content == null)
                {
                    _logger.LogError("Empty response body.");
                     throw new NetworkException("Empty response body");
                }

                return content;
            }
            catch (TaskCanceledException ex)
            {
                throw new NetworkException("Request timed out", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred", ex);
            }
        }

        public Task ExecuteRequest(HttpRequestMessage request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
