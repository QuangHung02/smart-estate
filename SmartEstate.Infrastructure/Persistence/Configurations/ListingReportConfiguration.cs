using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class ListingReportConfiguration : IEntityTypeConfiguration<ListingReport>
{
    public void Configure(EntityTypeBuilder<ListingReport> b)
    {
        b.ToTable("listing_reports");
        b.HasKey(x => x.Id);

        b.Property(x => x.Reason).HasMaxLength(300).IsRequired();
        b.Property(x => x.Detail).HasMaxLength(2000);

        b.HasIndex(x => new { x.ListingId, x.IsResolved });
        b.HasIndex(x => x.ReporterUserId);

        b.HasOne(x => x.Listing)
            .WithMany(x => x.Reports)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.ReporterUser)
            .WithMany()
            .HasForeignKey(x => x.ReporterUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.IsDeleted);
    }
}
