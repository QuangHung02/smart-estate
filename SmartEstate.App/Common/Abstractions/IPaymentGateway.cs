namespace SmartEstate.App.Common.Abstractions;

public sealed record PaymentInitResult(string Provider, string ProviderRef, string PayUrl);

public interface IPaymentGateway
{
    Task<PaymentInitResult> CreatePaymentAsync(
        Guid payerUserId,
        decimal amount,
        string currency,
        string description,
        CancellationToken ct = default
    );
}
