using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Network
{
    public class RequestBuilderOptions
    {
        public HttpMethod Method { get; init; }
        public string Url { get; init; } = default!;
        public object? Body { get; init; }
        public string? BearerToken { get; init; }
        public IDictionary<string, string>? Headers { get; init; }
    }
}
