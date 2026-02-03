using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> b)
    {
        b.ToTable("listings");
        b.HasKey(x => x.Id);

        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        b.Property(x => x.Description).HasMaxLength(5000).IsRequired();

        b.Property(x => x.PropertyType).IsRequired();

        // Money (owned)
        b.OwnsOne(x => x.Price, m =>
        {
            m.Property(p => p.Amount).HasColumnName("price_amount").HasColumnType("decimal(18,2)").IsRequired();
            m.Property(p => p.Currency).HasColumnName("price_currency").HasMaxLength(10).IsRequired();

            m.HasIndex(p => p.Amount);

        });

        b.Property(x => x.AreaM2);
        b.Property(x => x.Bedrooms);
        b.Property(x => x.Bathrooms);

        // AddressParts (owned)
        b.OwnsOne(x => x.Address, a =>
        {
            a.Property(p => p.FullAddress).HasColumnName("addr_full").HasMaxLength(500);
            a.Property(p => p.City).HasColumnName("addr_city").HasMaxLength(100);
            a.Property(p => p.District).HasColumnName("addr_district").HasMaxLength(100);
            a.Property(p => p.Ward).HasColumnName("addr_ward").HasMaxLength(100);
            a.Property(p => p.Street).HasColumnName("addr_street").HasMaxLength(200);

            a.HasIndex(p => p.City);
            a.HasIndex(p => p.District);
        });

        // GeoPoint (owned, optional)
        b.OwnsOne(x => x.Location, g =>
        {
            g.Property(p => p.Lat).HasColumnName("lat").HasColumnType("float");
            g.Property(p => p.Lng).HasColumnName("lng").HasColumnType("float");

            g.HasIndex(p => p.Lat);
            g.HasIndex(p => p.Lng);
        });
        b.Navigation(x => x.Location).IsRequired(false);

        b.Property(x => x.VirtualTourUrl).HasMaxLength(2000);

        b.Property(x => x.ModerationStatus).IsRequired();
        b.Property(x => x.ModerationReason).HasMaxLength(1000);
        b.Property(x => x.QualityScore).HasColumnType("decimal(5,2)");
        b.Property(x => x.AiFlagsJson).HasColumnType("nvarchar(max)");

        b.Property(x => x.LifecycleStatus).IsRequired();

        b.Property(x => x.CreatedByUserId).IsRequired();
        b.Property(x => x.ResponsibleUserId).IsRequired();
        b.Property(x => x.AssignedBrokerUserId);

        b.HasOne(x => x.AssignedBrokerUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedBrokerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // indexes for common queries (search)
        b.HasIndex(x => new { x.ModerationStatus, x.LifecycleStatus });
        b.HasIndex(x => x.ResponsibleUserId);

        b.HasIndex(x => x.IsDeleted);
    }
}
