using PaymentHub.Application.Dtos;

namespace PayPalIntegration.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(int userId, string frontendIdempotencyKey, List<CreateOrderItemRequest> items);
        Task<OrderResponse> GetOrderById(int orderId);
    }
}
