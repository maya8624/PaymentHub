using PaymentHub.Domain.Enums;
using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public PaymentProvider Provider { get; set; } 
        public string ProviderOrderId { get; set; } = null!;
        public string? ProviderCaptureId { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; } = Currency.AUD;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? CapturedAt { get; set; }

        public Order Order { get; set; } = null!;
    }
}
