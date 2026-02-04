namespace SmartEstate.Domain.ValueObjects;

public sealed class AddressParts
{
    public string? FullAddress { get; private set; }
    public string? City { get; private set; }
    public string? District { get; private set; }
    public string? Ward { get; private set; }
    public string? Street { get; private set; }

    private AddressParts() { }

    public AddressParts(string? fullAddress, string? city, string? district, string? ward, string? street)
    {
        FullAddress = fullAddress?.Trim();
        City = city?.Trim();
        District = district?.Trim();
        Ward = ward?.Trim();
        Street = street?.Trim();
    }
}
