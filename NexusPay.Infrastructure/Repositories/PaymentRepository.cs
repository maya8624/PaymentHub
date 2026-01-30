using Microsoft.EntityFrameworkCore;
using PaymentHub.Infrastructure.Interfaces;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Domain.Enums;
using PayPalIntegration.Infrastructure.Interfaces;
using PayPalIntegration.Infrastructure.Persistence;
using PayPalIntegration.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Infrastructure.Repositories
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        private readonly PayHubContext _context;

        public PaymentRepository(PayHubContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByProviderOrderId(string providerOrderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(x => x.ProviderOrderId == providerOrderId);
        }

        public async Task<Payment?> GetByOrderId(int orderId)
        {
            return await _context.Payments
                .SingleOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task<Payment?> GetPending(int orderId, string providerOrderId, PaymentStatus status)
        {
            return await _context.Payments
                .Where(x => x.OrderId == orderId)
                .Where(x => x.ProviderOrderId == providerOrderId)
                .Where(x => x.Status == status)
                .FirstOrDefaultAsync();
        }
    }
}
