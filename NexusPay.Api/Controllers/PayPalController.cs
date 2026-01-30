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

        [HttpPost("create-order")]
        public async Task<ActionResult<PayPalOrderResultResponse>> CreatePayPalOrder([FromBody] OrderPaymentRequest request)
        {
            var result = await _paypalService.CreateOrder(request.OrderId);
            return Ok(result);
        }

        [HttpPost("capture-order")]
        public async Task<ActionResult<PayPalCaptureResponse>> CaptureOrder([FromBody] OrderPaymentRequest request)
        {
            var result = await _paypalService.CaptureOrder(request.OrderId);
            return Ok(result);
        }        
        
        [HttpPost("refund")]
        public async Task<IActionResult> Refund(RefundRequest request, CancellationToken ct)
        {
            var refund = await _paypalService.RefundCapture(request.PaymentId, request.Amount, ct);
            return Ok(refund);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook(int orderId)
        {
            return Ok(); // Always return 200 quickly to acknowledge receipt
        }
    }
}
