using Microsoft.EntityFrameworkCore;
using PaymentHub.Infrastructure.Responses;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Infrastructure.Interfaces;
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
        private readonly PayHubContext _context;

        public OrderRepository(PayHubContext context) : base(context) 
        {
            _context = context;
        }
        
        public async Task<Order?> GetOrderByFrontendIdempontentKey(string key, int userId)
        { 
            return await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FrontendIdempotencyKey == key && x.UserId == userId);
        }

        public async Task<OrderForPaymentResponse?> GetOrderForPayment(int orderId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Id == orderId)
                .Select(o => new OrderForPaymentResponse
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    Currency = o.Currency,
                    FrontendIdempotencyKey = o.FrontendIdempotencyKey,
                    Items = o.Items.Select(i => new OrderItemForPaymentResponse
                    {
                        ProductName = i.ProductName,
                        UnitPrice = i.UnitPrice,
                        Quantity = i.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
