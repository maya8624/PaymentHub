using Microsoft.AspNetCore.Mvc;
using PaymentHub.Application.Dtos;
using PayPalIntegration.Application.Interfaces;

namespace PayPalIntegration.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
                
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
