using PaymentHub.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Exceptions
{
    public class PaymentException : Exception
    {
        public PaymentErrorCodes ErrorCode { get; }

        public PaymentException(PaymentErrorCodes errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public PaymentException(PaymentErrorCodes errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
