using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Network.Responses
{
    public class PayPalTokenResponse
    {
        public string AccessToken { get; set; } = default!;
        public string AppId { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string Scope { get; set; }
    }
}
