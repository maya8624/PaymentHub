using PaymentHub.Application.Interfaces;
using PaymentHub.Domain.Entities;
using PaymentHub.Domain.Enums;
using PaymentHub.Infrastructure.Interfaces;
using PayPalIntegration.Domain.Entities;
using PayPalIntegration.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentHub.Application.Services
{
    public class RefundService : IRefundService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IRefundRepository _refundRepo;
        private readonly IPayPalClient _payPalClient;

        public RefundService(IPaymentRepository paymentRepo, IRefundRepository refundRepo, IPayPalClient payPalClient)
        {
            _paymentRepo = paymentRepo;
            _refundRepo = refundRepo;
            _payPalClient = payPalClient;
        }

        public async Task<Refund> Refund(int paymentId, decimal amount, CancellationToken ct)
        {
            var payment = await _paymentRepo.GetByIdAsync(paymentId, ct)
                ?? throw new EntityNotFoundException(nameof(Payment), paymentId);

            if (payment.Status != PaymentStatus.Captured &&
                payment.Status != PaymentStatus.PartiallyRefunded)
                throw new InvalidOperationException("Payment is not refundable.");

            if (amount <= 0 || amount > payment.RefundableAmount)
                throw new InvalidOperationException("Invalid refund amount.");

            var idempotencyKey = Guid.NewGuid().ToString();

            var paypalResponse = await _payPalClient.RefundCaptureAsync(
                payment.PayPalCaptureId,
                amount,
                idempotencyKey,
                ct);

            var refund = new Refund
            {
                PaymentId = payment.Id,
                PayPalRefundId = paypalResponse.Id,
                Amount = amount,
                Status = RefundStatus.Completed,
                CreatedAt = DateTime.UtcNow,
                RawResponse = paypalResponse.Raw
            };

            await _refundRepo.AddAsync(refund, ct);

            payment.RefundedAmount += amount;
            payment.Status = payment.RefundableAmount == 0
                ? PaymentStatus.Refunded
                : PaymentStatus.PartiallyRefunded;

            await _paymentRepo.UpdateAsync(payment, ct);

            return refund;
        }
    }

}
