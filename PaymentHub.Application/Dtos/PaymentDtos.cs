namespace PayPalIntegration.Application.Dtos
{
    public record PaymentDto(
      Guid Id,
      string PayPalOrderId,
      decimal Amount,
      string Currency,
      string Status,
      DateTimeOffset CreatedAt,
      DateTimeOffset? CompletedAt
  );
}
