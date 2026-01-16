using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; } = Currency.AUD;
        public string OrderNumber { get; set; } = null!;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? CompletedAt { get; set; }

        public IReadOnlyCollection<Payment> Payments => _payments;
        private readonly List<Payment> _payments = new();

        public void AddPayment(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            _payments.Add(payment);
        }

        public void MarkCompleted()
        {
            Status = OrderStatus.Completed;
            CompletedAt = DateTimeOffset.UtcNow;
        }
    }
}
