using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class ListingImage : AuditableEntity
{
    public Guid ListingId { get; set; }
    public string Url { get; set; } = default!;
    public int SortOrder { get; set; }

    public string? Caption { get; set; }

    // Navigation
    public Listing Listing { get; set; } = default!;
}
