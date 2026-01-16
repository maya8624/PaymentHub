using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; } = Currency.AUD;
        public string PayPalOrderId { get; set; } = null!;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? CompletedAt { get; set; }

        public Order Order { get; set; } = null!;
    }
}
