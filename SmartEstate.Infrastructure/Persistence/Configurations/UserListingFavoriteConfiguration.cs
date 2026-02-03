using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public sealed class UserListingFavoriteConfiguration : IEntityTypeConfiguration<UserListingFavorite>
{
    public void Configure(EntityTypeBuilder<UserListingFavorite> b)
    {
        b.ToTable("user_listing_favorites");

        b.HasKey(x => x.Id);

        b.Property(x => x.UserId).IsRequired();
        b.Property(x => x.ListingId).IsRequired();

        // Unique: 1 user - 1 listing (no duplicates)
        b.HasIndex(x => new { x.UserId, x.ListingId }).IsUnique();

        b.HasOne(x => x.User)
            .WithMany(u => u.FavoriteListings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Listing)
            .WithMany(l => l.FavoritedByUsers)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Optional: helpful indexes
        b.HasIndex(x => x.UserId);
        b.HasIndex(x => x.ListingId);
    }
}
