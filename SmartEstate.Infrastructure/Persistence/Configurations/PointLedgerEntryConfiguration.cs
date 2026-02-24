using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class PointLedgerEntryConfiguration : IEntityTypeConfiguration<PointLedgerEntry>
{
    public void Configure(EntityTypeBuilder<PointLedgerEntry> b)
    {
        b.ToTable("point_ledger");
        b.HasKey(x => x.Id);

        b.Property(x => x.Reason).HasMaxLength(200).IsRequired();
        b.Property(x => x.RefType).HasMaxLength(50).IsRequired();

        b.HasIndex(x => x.UserId);
        b.HasIndex(x => new { x.UserId, x.CreatedAt });
        b.HasIndex(x => x.IsDeleted);
    }
}

