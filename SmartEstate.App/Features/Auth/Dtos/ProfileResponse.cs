namespace SmartEstate.App.Features.Auth.Dtos;

public sealed record ProfileResponse(
    Guid UserId,
    string Email,
    string DisplayName,
    string? Phone,
    string Role,
    bool IsActive,
    DateTimeOffset? LastLoginAt
);
