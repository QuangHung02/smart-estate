using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class ListingBoostConfiguration : IEntityTypeConfiguration<ListingBoost>
{
    public void Configure(EntityTypeBuilder<ListingBoost> b)
    {
        b.ToTable("listing_boosts");
        b.HasKey(x => x.Id);

        b.HasOne(x => x.Listing)
            .WithMany()
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.ListingId);
        b.HasIndex(x => new { x.ListingId, x.StartsAt, x.EndsAt });
        b.HasIndex(x => x.IsDeleted);
    }
}

