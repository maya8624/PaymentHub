using PaymentHub.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Network.Exceptions
{
    public class NetworkException : Exception
    {
        public ApiFailureReason ErrorCode { get; }

        public NetworkException() { }

        public NetworkException(string message) : base(message)
        { 
        }

        public NetworkException(string message, Exception inner) : base(message, inner)
        {
        }

        public NetworkException(ApiFailureReason errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public NetworkException(ApiFailureReason errorCode, string message, Exception inner)
            : base(message, inner)
        {
            ErrorCode = errorCode;
        }
    }
}
