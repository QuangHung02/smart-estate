using SmartEstate.Domain.Common;
using SmartEstate.Domain.Enums;

namespace SmartEstate.Domain.Entities;

public class PointPurchase : AuditableEntity
{
    public Guid UserId { get; set; }
    public Guid PointPackageId { get; set; }
    public int Points { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = default!;
    public PointPurchaseStatus Status { get; set; } = PointPurchaseStatus.Pending;
    public Guid? PaymentId { get; set; }

    public User User { get; set; } = default!;
    public PointPackage PointPackage { get; set; } = default!;
    public Payment? Payment { get; set; }
}

