using PaymentHub.Application.Dtos;
using PaymentHub.Application.Exceptions;
using PaymentHub.Domain.Entities;
using PaymentHub.Infrastructure.Responses;
using PayPalIntegration.Application.Interfaces;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Infrastructure.Interfaces;

namespace PayPalIntegration.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _uow;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork uow)
        {
            _orderRepository = orderRepository;
            _uow = uow;
        }

        public async Task<OrderResponse> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            if (order == null)
                throw new NotFoundException("Order is not found.");

            var response = new OrderResponse
            {
                OrderId = order.Id,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(x => new OrderItemResponse
                {
                    ProductId = x.ProductId, 
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice,
                }).ToList()
            };

            return response;
        }

        public async Task<IEnumerable<OrderSummaryResponse>> GetOrdersForUser(int userId)
        {
            var orders = await _orderRepository.GetOrdersForUser(userId);
            return orders;
        }

        public async Task<OrderResponse> CreateOrder(int userId, string frontendIdempotencyKey, List<CreateOrderItemRequest> items)
        {
            var existing = await _orderRepository
                .GetOrderByFrontendIdempontentKey(frontendIdempotencyKey, userId);

             if (existing != null)
                return MapToOrderResponse(existing);

            var orderItems = items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            }).ToList();

            var totalAmount = items.Sum(x => x.Quantity * x.UnitPrice);
            
            var order = new Order
            {
                UserId = userId,
                FrontendIdempotencyKey = frontendIdempotencyKey,
                TotalAmount = totalAmount,
                Items = orderItems
            };

            await _orderRepository.Create(order);
            await _uow.SaveChanges();

            return MapToOrderResponse(order);
        }

        private static OrderResponse MapToOrderResponse(Order order)
        {
            return new OrderResponse
            {
                OrderId = order.Id,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                IdempotencyKey = order.FrontendIdempotencyKey,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(x => new OrderItemResponse
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.Quantity * x.UnitPrice
                }).ToList()
            };
        }
    }
}
