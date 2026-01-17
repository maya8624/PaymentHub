using PaymentHub.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Exceptions
{
    public class PaymentSaveException : Exception
    {
        public ApplicationErrorCodes ErrorCode { get; }

        public PaymentSaveException(ApplicationErrorCodes errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public PaymentSaveException(ApplicationErrorCodes errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
