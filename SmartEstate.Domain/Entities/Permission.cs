using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class Permission : AuditableEntity
{
    public short Id { get; set; }
    public string Code { get; set; } = default!;
    public string? Description { get; set; }
}
