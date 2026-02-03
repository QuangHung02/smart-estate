namespace SmartEstate.App.Features.Auth.Dtos;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string DisplayName
);
