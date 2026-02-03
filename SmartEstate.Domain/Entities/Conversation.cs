using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class Conversation : AuditableEntity
{
    public Guid ListingId { get; set; }

    // buyer initiates conversation
    public Guid BuyerUserId { get; set; }

    // snapshot of responsible at creation (optional, but useful)
    public Guid ResponsibleUserId { get; set; }

    public DateTimeOffset? LastMessageAt { get; set; }
    public string? LastMessagePreview { get; set; }

    // Navigation
    public Listing Listing { get; set; } = default!;
    public User BuyerUser { get; set; } = default!;
    public User ResponsibleUser { get; set; } = default!;

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
