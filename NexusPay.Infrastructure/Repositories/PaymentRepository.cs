using Microsoft.EntityFrameworkCore;
using NexusPay.Infrastructure.Interfaces;
using NexusPay.Domain.Entities;
using NexusPay.Domain.Enums;
using NexusPay.Infrastructure.Interfaces;
using NexusPay.Infrastructure.Persistence;
using NexusPay.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusPay.Infrastructure.Repositories
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        private readonly NexusPayContext _context;

        public PaymentRepository(NexusPayContext context) : base(context)
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
