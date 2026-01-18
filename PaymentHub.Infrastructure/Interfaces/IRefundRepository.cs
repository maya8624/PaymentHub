using PaymentHub.Domain.Entities;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Infrastructure.Interfaces;
using System;

namespace PaymentHub.Infrastructure.Interfaces
{
    public interface IRefundRepository : IRepositoryBase<Refund>
    {
    }
}
