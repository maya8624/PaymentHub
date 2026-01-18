using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentHub.Network
{
    public static class HttpRequestFactory
    {
        public static HttpRequestMessage CreateJson(
            HttpMethod method,
            string url,
            object? body = null,
            string? bearerToken = null,
            IDictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (string.IsNullOrWhiteSpace(bearerToken) == false)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }

            if (headers != null)
            {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (body != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            }

            return request;
        }
    }
}
