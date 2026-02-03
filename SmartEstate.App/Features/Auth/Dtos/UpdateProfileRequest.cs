namespace SmartEstate.App.Features.Auth.Dtos;

public sealed record UpdateProfileRequest(
    string DisplayName,
    string? Phone,
    string? CurrentPassword,
    string? NewPassword
);
