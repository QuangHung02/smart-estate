using SmartEstate.Domain.Common;
using SmartEstate.Domain.Enums;

namespace SmartEstate.Domain.Entities;

public class ModerationReport : AuditableEntity
{
    public Guid ListingId { get; set; }
    public int Score { get; set; }
    public string? FlagsJson { get; set; }
    public string? SuggestionsJson { get; set; }
    public string Decision { get; set; } = default!;
    public Guid? ReviewedByAdminId { get; set; }
    public DateTimeOffset? ReviewedAt { get; set; }
    public ModerationStatus? ReviewOutcome { get; set; }
    public Listing Listing { get; set; } = default!;

    public static ModerationReport CreateFromAiDecision(
        Guid listingId,
        decimal? qualityScore,
        string decision,
        string? reason,
        string? flagsJson)
    {
        var score = qualityScore.HasValue ? (int)Math.Round(qualityScore.Value, 0) : 0;

        return new ModerationReport
        {
            ListingId = listingId,
            Score = score,
            FlagsJson = flagsJson,
            SuggestionsJson = string.IsNullOrWhiteSpace(reason) ? null : reason,
            Decision = decision
        };
    }

    public void MarkReviewed(Guid adminUserId, DateTimeOffset at, ModerationStatus outcome)
    {
        ReviewedByAdminId = adminUserId;
        ReviewedAt = at;
        ReviewOutcome = outcome;
    }
}

