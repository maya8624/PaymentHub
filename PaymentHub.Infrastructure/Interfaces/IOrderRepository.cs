using PaymentHub.Infrastructure.Responses;
using PayPalIntegration.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalIntegration.Infrastructure.Interfaces
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<OrderForPaymentResponse> GetOrderForPayment(int orderId);

        Task<Order> GetOrderByFrontendIdempontentKey(string key, int userId);
    }
}
