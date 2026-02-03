namespace SmartEstate.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "VND";

    private Money() { }

    public Money(decimal amount, string currency = "VND")
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        Amount = amount;
        Currency = string.IsNullOrWhiteSpace(currency) ? "VND" : currency.Trim().ToUpperInvariant();
    }

    public override string ToString() => $"{Amount:n0} {Currency}";
}
