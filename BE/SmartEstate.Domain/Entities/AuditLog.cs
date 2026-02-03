using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class AuditLog : AuditableEntity
{
    public Guid ActorUserId { get; set; }          // admin/system
    public string Action { get; set; } = default!; // e.g. "MODERATION_APPROVE", "PAYMENT_PAID"

    public string EntityType { get; set; } = default!; // "Listing", "Payment", ...
    public Guid? EntityId { get; set; }

    public string? MetadataJson { get; set; }      // extra info
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    // Navigation
    public User ActorUser { get; set; } = default!;
}
