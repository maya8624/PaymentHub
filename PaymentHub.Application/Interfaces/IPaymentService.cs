using PayPalIntegration.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Interfaces
{
    public interface IPaymentService
    {
        Task SavePayment(Payment payment);
        Task UpdatePayment(Payment payment);
    }
}
