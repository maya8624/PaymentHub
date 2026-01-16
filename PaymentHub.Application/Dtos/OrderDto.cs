using PayPalIntegration.Domain.Enums;

namespace PayPalIntegration.Application.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
    }
}
