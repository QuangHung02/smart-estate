using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.Auth.Dtos;

public sealed record ProfileResponse(
    Guid UserId,
    string Email,
    string DisplayName,
    string? Phone,
    UserRole Role,
    bool IsActive,
    DateTimeOffset? LastLoginAt
);
