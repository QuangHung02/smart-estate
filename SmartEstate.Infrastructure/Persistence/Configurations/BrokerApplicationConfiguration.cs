using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class BrokerApplicationConfiguration : IEntityTypeConfiguration<BrokerApplication>
{
    public void Configure(EntityTypeBuilder<BrokerApplication> b)
    {
        b.ToTable("broker_applications");
        b.HasKey(x => x.Id);

        b.Property(x => x.DocUrl).HasMaxLength(2000);

        b.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.UserId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.IsDeleted);
    }
}

