using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Application.Dtos
{
    //    public record OrderDto(
    //      Guid Id,
    //      string OrderNumber,
    //      decimal Amount,
    //      Currency Currency,
    //      string Status,
    //      DateTimeOffset CreatedAt
    //      //PaymentDto? Payment
    //  );

    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
    }
}
