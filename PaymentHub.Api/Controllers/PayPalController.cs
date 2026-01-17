using Microsoft.AspNetCore.Mvc;
using PaymentHub.Application.Dtos;
using PaymentHub.Application.Interfaces;

namespace PaymentHub.Api.Controllers
{
    [ApiController]
    [Route("api/paypal")]
    public class PayPalController : ControllerBase
    {
        private readonly IPayPalService _paypalService;

        public PayPalController(IPayPalService paypalService)
        {
            _paypalService = paypalService;
        }

        [HttpPost("create-order/{orderId:int}")]
        public async Task<IActionResult> CreatePayPalOrder(int orderId)
        {
            var paypalOrderId = await _paypalService.CreateOrder(orderId);
            return Ok(new { paypalOrderId });
        }

        [HttpPost("capture-order/{orderId:int}")]
        public async Task<ActionResult<PayPalCaptureResponse>> CaptureOrder(int orderId)
        {
            var result = await _paypalService.CaptureOrder(orderId);
            return Ok(result);
        }
    }
}
