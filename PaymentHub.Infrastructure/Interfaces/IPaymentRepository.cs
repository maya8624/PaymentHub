using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Infrastructure.Interfaces
{
    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
    }
}
