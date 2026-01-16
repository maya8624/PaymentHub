using PayPalIntegration.Application.Requests;
using PayPalIntegration.Domain.Entities;

namespace PayPalIntegration.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(CreateOrderRequest request);
    }
}
