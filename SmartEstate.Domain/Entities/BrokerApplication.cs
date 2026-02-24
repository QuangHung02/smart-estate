using SmartEstate.Domain.Common;
using SmartEstate.Domain.Enums;

namespace SmartEstate.Domain.Entities;

public class BrokerApplication : AuditableEntity
{
    public Guid UserId { get; set; }
    public string? DocUrl { get; set; }
    public BrokerApplicationStatus Status { get; set; } = BrokerApplicationStatus.Pending;
    public Guid? ReviewedByAdminId { get; set; }
    public DateTimeOffset? ReviewedAt { get; set; }
    public bool IsActivationPaid { get; set; }
    public DateTimeOffset? ActivationPaidAt { get; set; }

    public User User { get; set; } = default!;
}

