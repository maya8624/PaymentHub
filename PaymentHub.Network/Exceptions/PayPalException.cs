using PaymentHub.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Network.Exceptions
{
    public class PayPalException : Exception
    {
        public PayPalErrorCodes ErrorCode { get; }

        public PayPalException() { }

        public PayPalException(PayPalErrorCodes errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public PayPalException(PayPalErrorCodes errorCode, string message, Exception innerException) 
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
