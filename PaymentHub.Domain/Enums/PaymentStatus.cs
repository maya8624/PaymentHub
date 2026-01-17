using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalIntegration.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Captured,
        Failed,
        Refunded,
        Rejected
    }
}
