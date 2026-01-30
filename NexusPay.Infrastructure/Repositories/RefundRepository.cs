using PaymentHub.Domain.Entities;
using PaymentHub.Infrastructure.Interfaces;
using PayPalIntegration.Infrastructure.Persistence;
using PayPalIntegration.Infrastructure.Repositories;

namespace PaymentHub.Infrastructure.Repositories
{
    public class RefundRepository : RepositoryBase<Refund>, IRefundRepository
    {
        private readonly PayHubContext _context;

        public RefundRepository(PayHubContext context) : base(context)
        {
            _context = context;
        }
    }
}
