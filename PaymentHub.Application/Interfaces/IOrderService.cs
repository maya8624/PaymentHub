using PaymentHub.Application.Dtos;
using PaymentHub.Infrastructure.Responses;

namespace PayPalIntegration.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(int userId, string frontendIdempotencyKey, List<CreateOrderItemRequest> items);
        Task<OrderResponse> GetOrderById(int orderId);
        Task<IEnumerable<OrderSummaryResponse>> GetOrdersForUser(int userId);
    }
}
