using SmartEstate.App.Common.Abstractions;

namespace SmartEstate.Api.Integrations;

public sealed class DummyPaymentGateway : IPaymentGateway
{
    public Task<PaymentInitResult> CreatePaymentAsync(Guid payerUserId, decimal amount, string currency, string description, CancellationToken ct = default)
    {
        // demo: trả url giả
        var provider = "dummy";
        var providerRef = Guid.NewGuid().ToString("N");
        var payUrl = $"/api/payments/dummy/{providerRef}";
        return Task.FromResult(new PaymentInitResult(provider, providerRef, payUrl));
    }
}
