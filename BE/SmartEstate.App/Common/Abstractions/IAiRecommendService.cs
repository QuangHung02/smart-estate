namespace SmartEstate.App.Common.Abstractions;

public sealed record RecommendItem(Guid ListingId, decimal Score, string Explanation);

public interface IAiRecommendService
{
    Task<IReadOnlyList<RecommendItem>> RecommendAsync(
        Guid buyerUserId,
        string criteriaJson,
        CancellationToken ct = default
    );
}
