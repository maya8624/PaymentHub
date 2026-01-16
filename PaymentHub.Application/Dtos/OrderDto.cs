namespace PayPalIntegration.Application.Dtos
{
    public record OrderDto(
      Guid Id,
      string OrderNumber,
      decimal Amount,
      string Currency,
      string Status,
      DateTimeOffset CreatedAt,
      PaymentDto? Payment
  );
}
