using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class ListingImageConfiguration : IEntityTypeConfiguration<ListingImage>
{
    public void Configure(EntityTypeBuilder<ListingImage> b)
    {
        b.ToTable("listing_images");
        b.HasKey(x => x.Id);

        b.Property(x => x.Url).HasMaxLength(2000).IsRequired();
        b.Property(x => x.Caption).HasMaxLength(300);

        b.HasIndex(x => new { x.ListingId, x.SortOrder });

        b.HasOne(x => x.Listing)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.IsDeleted);
    }
}
