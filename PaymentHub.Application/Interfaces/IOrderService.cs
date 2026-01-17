using PaymentHub.Application.Dtos;
using PayPalIntegration.Application.Dtos;
using PayPalIntegration.Domain.Entities;

namespace PayPalIntegration.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(int usrId, string frontendIdempotencyKey, List<CreateOrderItemRequest> items);
    }
}
