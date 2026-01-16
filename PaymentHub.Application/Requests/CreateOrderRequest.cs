using FluentValidation;
using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Application.Requests
{
    public class CreateOrderRequest
    {
        public Currency Currency { get; set; } = Currency.AUD;
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }

    public class BusinessValidator : AbstractValidator<CreateOrderRequest>
    {
        public BusinessValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty();
        }
    }
}
