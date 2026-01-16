using PaymentHub.Application.Constants;
using PaymentHub.Application.Services;
using PayPalIntegration.Application.Dtos;
using PayPalIntegration.Application.Interfaces;
using PayPalIntegration.Application.Requests;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Domain.Enums;
using PayPalIntegration.Domain.Interfaces;

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

        public async Task<OrderDto> CreateOrder(CreateOrderRequest request)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = OrderNumberGenerator.Generate(),
                Amount = request.Quantity * ProductPrices.ProductPriceA,   
                Currency = request.Currency,
                Status = OrderStatus.Pending,
                CreatedAt = DateTimeOffset.UtcNow,
                //UserId = request.UserId
            };

            await _orderRepository.Create(order);
            await _uow.SaveChanges();

            var result = new OrderDto
            {
                Id = order.Id,
                //    order.OrderNumber,
                //    order.Amount,
                //    order.Currency,
                //    order.Status.ToString(),
                //    order.CreatedAt

            };

            return result;
        }
    }
}
