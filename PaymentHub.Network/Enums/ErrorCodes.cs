using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Network.Enums
{
    public enum ErrorCodes
    {
        PayPalTokenMissing = 1000,
        PayPalTokenRequestFailed = 1001,
        PayPalUnauthorized = 1002,
        PayPalRateLimited = 1003,
        PayPalServerError = 1004,
        PayPalUnknown = 1005
    }
}
