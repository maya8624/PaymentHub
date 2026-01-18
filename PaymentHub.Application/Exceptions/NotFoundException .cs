using PaymentHub.Application.Enums;
using PaymentHub.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public ApplicationErrorCodes ErrorCode { get; }

        public NotFoundException () { }

        public NotFoundException(ApplicationErrorCodes errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public NotFoundException(ApplicationErrorCodes errorCode, string message, Exception inner)
            : base(message, inner)
        {
            ErrorCode = errorCode;
        }
    }
}
