using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class BrokerProfileConfiguration : IEntityTypeConfiguration<BrokerProfile>
{
    public void Configure(EntityTypeBuilder<BrokerProfile> b)
    {
        b.ToTable("broker_profiles");
        b.HasKey(x => x.Id);

        b.Property(x => x.CompanyName).HasMaxLength(200);
        b.Property(x => x.LicenseNo).HasMaxLength(100);
        b.Property(x => x.Bio).HasMaxLength(2000);

        b.Property(x => x.RatingAvg).HasColumnType("decimal(5,2)");
        b.Property(x => x.RatingCount);

        b.HasIndex(x => x.UserId).IsUnique();
    }
}
