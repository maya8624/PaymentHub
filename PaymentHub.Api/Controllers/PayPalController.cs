using Microsoft.AspNetCore.Mvc;
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

        //[HttpPost("paypal/capture-order/{orderId}")]
        //public async Task<IActionResult> CaptureOrder(int orderId)
        //{
        //    var result = await _paypalService.CapturePayPalOrderAsync(orderId);
        //    return Ok(result);
        //}
    }
}
