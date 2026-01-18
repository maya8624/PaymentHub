using Microsoft.AspNetCore.Mvc;
using PaymentHub.Application.Dtos;
using PaymentHub.Application.Interfaces;
using PaymentHub.Application.Services;
using System.Text.Json;

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

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook(int orderId)
        {
            //var headers = Request.Headers;

            //// 2. Read the raw body as a string (Verification requires the exact raw body)
            //using var reader = new StreamReader(Request.Body);
            //var body = await reader.ReadToEndAsync();

            //// 3. Verify the signature via your service
            //var isValid = await _payPalService.VerifyWebhookSignature(headers, body);

            //if (!isValid)
            //{
            //    _logger.LogWarning("Invalid PayPal Webhook signature received.");
            //    return BadRequest();
            //}

            //// 4. Parse and process the event
            //var ev = JsonSerializer.Deserialize<PayPalWebhookEvent>(body);
            //await _payPalService.HandleWebhookEvent(ev);

            return Ok(); // Always return 200 quickly to acknowledge receipt
        }
    }
}
