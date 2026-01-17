using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Network.Enums
{
    public enum PayPalErrorCodes
    {
        PayPalAccessTokenFailed = 1001,
        PayPalCreateOrderFailed = 1002,
        PayPalUnauthorized = 1003,
        PayPalCaptureFailed = 1004,
        PayPalUnknown = 1005
    }
}
