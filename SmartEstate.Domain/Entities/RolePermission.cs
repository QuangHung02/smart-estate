using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class RolePermission : AuditableEntity
{
    public short RoleId { get; set; }
    public short PermissionId { get; set; }

    public Role Role { get; set; } = default!;
    public Permission Permission { get; set; } = default!;
}
