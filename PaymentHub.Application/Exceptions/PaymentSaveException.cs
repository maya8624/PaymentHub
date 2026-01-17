using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Exceptions
{
    public class PaymentSaveException : Exception
    {
        public int OrderId { get; }

        public PaymentSaveException(int orderId, string message)
            : base(message)
        {
            OrderId = orderId;
        }

        public PaymentSaveException(int orderId, string message, Exception innerException)
            : base(message, innerException)
        {
            OrderId = orderId;
        }
    }
}
