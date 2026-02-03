namespace SmartEstate.Api.Security;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "SmartEstate";
    public string Audience { get; set; } = "SmartEstate";
    public string Key { get; set; } = default!;
    public int ExpiresMinutes { get; set; } = 60 * 24 * 7; // default 7 days
}
