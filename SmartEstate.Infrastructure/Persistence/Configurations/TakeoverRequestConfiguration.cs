using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public sealed class TakeoverRequestConfiguration : IEntityTypeConfiguration<TakeoverRequest>
{
    public void Configure(EntityTypeBuilder<TakeoverRequest> b)
    {
        b.ToTable("takeover_requests");

        b.HasKey(x => x.Id);

        b.Property(x => x.Status).IsRequired();
        b.Property(x => x.Payer).IsRequired();

        b.OwnsOne(x => x.Fee, money =>
        {
            money.Property(p => p.Amount).HasColumnName("fee_amount").HasColumnType("decimal(18,2)").IsRequired();
            money.Property(p => p.Currency).HasColumnName("fee_currency").HasMaxLength(8).IsRequired();
        });

        b.Property(x => x.Note).HasMaxLength(500);

        b.HasOne(x => x.Listing)
            .WithMany(x => x.TakeoverRequests)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.SellerUser)
            .WithMany()
            .HasForeignKey(x => x.SellerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.BrokerUser)
            .WithMany()
            .HasForeignKey(x => x.BrokerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Payment)
            .WithMany()
            .HasForeignKey(x => x.PaymentId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasIndex(x => new { x.ListingId, x.Status });
        b.HasIndex(x => new { x.BrokerUserId, x.Status });
    }
}
