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
        public static HttpRequestMessage CreateJson(RequestBuilderOptions options)
        {
            var request = new HttpRequestMessage(options.Method, options.Url);

            if (string.IsNullOrWhiteSpace(options.BearerToken) == false)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.BearerToken);
            }

            if (options.Headers != null)
            {
                foreach (var header in options.Headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (options.Body != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(options.Body), Encoding.UTF8, "application/json");
            }

            return request;
        }
    }
}
