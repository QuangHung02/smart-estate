using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("users");
        b.HasKey(x => x.Id);

        b.Property(x => x.Email).HasMaxLength(256).IsRequired();
        b.HasIndex(x => x.Email).IsUnique();

        b.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
        b.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
        b.Property(x => x.Phone).HasMaxLength(50);

        b.Property(x => x.RoleId).IsRequired();
        b.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        b.Property(x => x.IsActive).IsRequired();

        // soft delete
        b.HasIndex(x => x.IsDeleted);

        // 1-1 broker profile
        b.HasOne(x => x.BrokerProfile)
            .WithOne(x => x.User)
            .HasForeignKey<BrokerProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // created listings
        b.HasMany(x => x.CreatedListings)
            .WithOne(x => x.CreatedByUser)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // responsible listings
        b.HasMany(x => x.ResponsibleListings)
            .WithOne(x => x.ResponsibleUser)
            .HasForeignKey(x => x.ResponsibleUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // conversations as buyer
        b.HasMany(x => x.BuyerConversations)
            .WithOne(x => x.BuyerUser)
            .HasForeignKey(x => x.BuyerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // messages sent
        b.HasMany(x => x.MessagesSent)
            .WithOne(x => x.SenderUser)
            .HasForeignKey(x => x.SenderUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
