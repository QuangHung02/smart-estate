using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> b)
    {
        b.ToTable("audit_logs");
        b.HasKey(x => x.Id);

        b.Property(x => x.Action).HasMaxLength(150).IsRequired();
        b.Property(x => x.EntityType).HasMaxLength(100).IsRequired();
        b.Property(x => x.MetadataJson).HasColumnType("nvarchar(max)");
        b.Property(x => x.IpAddress).HasMaxLength(64);
        b.Property(x => x.UserAgent).HasMaxLength(400);

        b.HasIndex(x => new { x.EntityType, x.EntityId });
        b.HasIndex(x => x.ActorUserId);

        b.HasOne(x => x.ActorUser)
            .WithMany()
            .HasForeignKey(x => x.ActorUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.IsDeleted);
    }
}
