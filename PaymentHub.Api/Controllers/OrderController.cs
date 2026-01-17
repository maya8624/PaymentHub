using Microsoft.AspNetCore.Mvc;
using PaymentHub.Application.Dtos;
using PayPalIntegration.Application.Interfaces;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /*
            Frontend
               ↓
            Backend creates Order (Pending)
               ↓
            Backend creates PayPal Order
               ↓
            Store PayPalOrderId in DB
               ↓
            Frontend approves payment
               ↓
            Backend captures payment
               ↓
            Mark Order as Paid
               ↓
            Webhook confirms final state
         */
        
        [HttpPost("create")]
        public async Task<ActionResult<OrderResponse>> Create([FromBody] CreateOrderRequest request)
        {
            var order = await _orderService.CreateOrder(
                request.UserId, 
                request.FrontendIdempotencyKey, 
                request.Items);

            return Ok(order);
        }
    }
}
