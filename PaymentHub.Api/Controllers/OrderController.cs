using Microsoft.AspNetCore.Mvc;
using PaymentHub.Application.Dtos;
using PaymentHub.Infrastructure.Responses;
using PayPalIntegration.Application.Interfaces;
using System.Security.Claims;

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

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            return Ok(order);
        }

        [HttpGet("{orders}")]
        public async Task<ActionResult<List<OrderSummaryResponse>>> GetOrders()
        {
            // get userId from JWT claims
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userId == null) return Unauthorized();

            var userId = 1;
            var orders = await _orderService.GetOrdersForUser(userId);

            return Ok(orders);
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
