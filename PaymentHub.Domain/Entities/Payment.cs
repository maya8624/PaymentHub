using PaymentHub.Domain.Enums;
using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public PaymentProvider Provider { get; set; } 
        public string ProviderOrderId { get; set; } = null!;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        //public string? ProviderCaptureId { get; set; }
        public decimal Amount { get; set; }
        public string BackendIdempotencyKey { get; set; } = Guid.NewGuid().ToString();
        public Currency Currency { get; set; } = Currency.AUD;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        //public DateTimeOffset? CapturedAt { get; set; }

        public Order Order { get; set; } = null!;
    }
}
