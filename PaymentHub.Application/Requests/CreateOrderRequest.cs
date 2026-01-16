using FluentValidation;
using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Application.Requests
{
    public record CreateOrderRequest(decimal Amount, Currency Currency = Currency.AUD);

    public class BusinessValidator : AbstractValidator<CreateOrderRequest>
    {
        public BusinessValidator()
        {
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty();
        }
    }
}
