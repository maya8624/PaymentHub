using Microsoft.AspNetCore.Mvc;
using PayPalIntegration.Application.Requests;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class OrderController : ControllerBase
    {
        public OrderController()
        {
            
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = $"ORD-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}",
                Amount = request.Amount,
                Currency = request.Currency,
                Status = OrderStatus.Pending,
                CreatedAt = DateTimeOffset.UtcNow
            };

            return Ok();
        }
    }
}
