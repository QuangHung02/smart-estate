namespace SmartEstate.Domain.Common;

public static class Guards
{
    public static void AgainstNullOrEmpty(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{field} cannot be empty.");
    }

    public static void AgainstNegative(decimal value, string field)
    {
        if (value < 0)
            throw new DomainException($"{field} cannot be negative.");
    }

    public static void AgainstDefaultGuid(Guid value, string paramName)
    {
        if (value == Guid.Empty)
            throw new DomainException($"{paramName} must not be an empty Guid.");
    }
}
