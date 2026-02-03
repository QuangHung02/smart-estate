using SmartEstate.App.Common.Abstractions;

namespace SmartEstate.Api.Integrations;

public sealed class DummyAiModerationService : IAiModerationService
{
    public Task<ModerationDecision> ModerateListingAsync(string title, string description, CancellationToken ct = default)
    {
        // TODO: thay bằng call model S2 thật
        // heuristic: nếu quá ngắn => need_review
        if (title.Length < 5 || description.Length < 20)
            return Task.FromResult(new ModerationDecision("NEED_REVIEW", 40, "Content too short.", "{\"flags\":[\"too_short\"]}"));

        return Task.FromResult(new ModerationDecision("AUTO_APPROVE", 85, null, "{\"flags\":[]}"));
    }
}
