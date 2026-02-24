using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class PointPackage : AuditableEntity
{
    public string Name { get; set; } = default!;
    public int Points { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = default!;
    public bool IsActive { get; set; } = true;
}

