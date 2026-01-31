using NexusPay.Infrastructure.Responses;
using NexusPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusPay.Infrastructure.Interfaces
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<Order?> GetOrderById(int orderId);
        Task<OrderForPaymentResponse> GetOrderForPayment(int orderId);
        Task<IEnumerable<OrderSummaryResponse>> GetOrdersForUser(int userId);
        Task<Order> GetOrderByFrontendIdempontentKey(string key, int userId);
    }
}
