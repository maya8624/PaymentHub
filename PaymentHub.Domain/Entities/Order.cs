using PaymentHub.Domain.Entities;
using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Currency Currency { get; set; } = Currency.AUD;
        public string FrontendIdempotencyKey { get; set; } 
        public int UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<OrderItem> Items { get; set; } = [];
        public Payment Payment { get; set; }

        //public IReadOnlyCollection<Payment> Payments => _payments;
        //private readonly List<Payment> _payments = new();

        //public void AddPayment(Payment payment)
        //{
        //    if (payment == null)
        //        throw new ArgumentNullException(nameof(payment));

        //    _payments.Add(payment);
        //}

        //public void MarkCompleted()
        //{
        //    Status = OrderStatus.Completed;
        //    CompletedAt = DateTimeOffset.UtcNow;
        //}
    }
}
