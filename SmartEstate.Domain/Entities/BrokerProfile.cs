using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class BrokerProfile : AuditableEntity
{
    public Guid UserId { get; set; }
    public string? CompanyName { get; set; }
    public string? LicenseNo { get; set; }
    public string? Bio { get; set; }

    public decimal RatingAvg { get; set; }
    public int RatingCount { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}
