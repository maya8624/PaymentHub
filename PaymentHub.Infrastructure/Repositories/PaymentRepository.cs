using PaymentHub.Infrastructure.Interfaces;
using PayPalIntegration.Domain.Entities;
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
    }
}
