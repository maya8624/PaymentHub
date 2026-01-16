using PayPalIntegration.Application.Dtos;
using PayPalIntegration.Application.Requests;
using PayPalIntegration.Domain.Entities;

namespace PayPalIntegration.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(CreateOrderRequest request);
    }
}
