using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.Moderation.Dtos;

public sealed record ModerationReportDto(
    Guid Id,
    int Score,
    string Decision,
    string? FlagsJson,
    string? SuggestionsJson,
    Guid? ReviewedByAdminId,
    DateTimeOffset? ReviewedAt,
    ModerationStatus? ReviewOutcome);

public sealed record PendingListingModerationItemDto(
    Guid ListingId,
    string Title,
    string? City,
    string? District,
    ModerationStatus ModerationStatus,
    ListingLifecycleStatus LifecycleStatus,
    DateTimeOffset CreatedAt,
    ModerationReportDto? LatestReport);

