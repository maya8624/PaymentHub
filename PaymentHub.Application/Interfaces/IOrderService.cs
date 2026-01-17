using PaymentHub.Application.Dtos;

namespace PayPalIntegration.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(int usrId, string frontendIdempotencyKey, List<CreateOrderItemRequest> items);
    }
}
