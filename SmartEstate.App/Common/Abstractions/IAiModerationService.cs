namespace SmartEstate.App.Common.Abstractions;

public sealed record ModerationDecision(
    string Decision,          // "AUTO_APPROVE" | "AUTO_REJECT" | "NEED_REVIEW"
    decimal? QualityScore,
    string? Reason,
    string? FlagsJson
);

public interface IAiModerationService
{
    Task<ModerationDecision> ModerateListingAsync(string title, string description, CancellationToken ct = default);
}
