using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Responses
{
    public class PayPalTokenResponse
    {
        public string AccessToken { get; set; } = default!;
        public int ExpiresIn { get; set; }
    }
}
