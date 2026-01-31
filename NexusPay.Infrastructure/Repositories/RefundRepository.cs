using NexusPay.Domain.Entities;
using NexusPay.Infrastructure.Interfaces;
using NexusPay.Infrastructure.Persistence;
using NexusPay.Infrastructure.Repositories;

namespace NexusPay.Infrastructure.Repositories
{
    public class RefundRepository : RepositoryBase<Refund>, IRefundRepository
    {
        private readonly NexusPayContext _context;

        public RefundRepository(NexusPayContext context) : base(context)
        {
            _context = context;
        }
    }
}
