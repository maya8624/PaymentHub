using Microsoft.EntityFrameworkCore;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Domain.Interfaces;
using PayPalIntegration.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalIntegration.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly PayPalDbContext _context;

        public OrderRepository(PayPalDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Order?> GetByOrderNumber(string orderNumber)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);
        }
    }
}
