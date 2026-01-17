using PaymentHub.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Network.Exceptions
{
    public class PayPalAuthenticationException : Exception
    {
        public ErrorCodes ErrorCode { get; }

        public PayPalAuthenticationException(ErrorCodes errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public PayPalAuthenticationException(ErrorCodes errorCode, string message, Exception innerException) 
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
