using NexusPay.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusPay.Application.Interfaces
{
    public interface IPayPalService
    {
        Task<PayPalOrderResultResponse> CreateOrder(int orderId);
        Task<PayPalCaptureResponse> CaptureOrder(int orderId);
        Task<PayPalRefundResponse> RefundCapture(int paymentId, decimal amount, CancellationToken ct);
    }
}
