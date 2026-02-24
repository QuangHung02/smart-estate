using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class ListingBoost : AuditableEntity
{
    public Guid ListingId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset StartsAt { get; set; }
    public DateTimeOffset EndsAt { get; set; }

    public Listing Listing { get; set; } = default!;
    public User User { get; set; } = default!;
}

