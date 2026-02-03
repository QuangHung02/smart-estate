using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class ListingReport : AuditableEntity
{
    public Guid ListingId { get; set; }
    public Guid ReporterUserId { get; set; }

    public string Reason { get; set; } = default!;
    public string? Detail { get; set; }

    public bool IsResolved { get; set; }
    public DateTimeOffset? ResolvedAt { get; set; }
    public Guid? ResolvedByAdminId { get; set; }
    public string? ResolutionNote { get; set; }

    // Navigation
    public Listing Listing { get; set; } = default!;
    public User ReporterUser { get; set; } = default!;
}
