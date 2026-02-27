using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> b)
    {
        b.ToTable("conversations");
        b.HasKey(x => x.Id);

        b.Property(x => x.LastMessagePreview).HasMaxLength(300);

        // 1 buyer per listing => unique
        b.HasIndex(x => new { x.ListingId, x.BuyerUserId }).IsUnique();

        b.HasOne(x => x.Listing)
            .WithMany(x => x.Conversations)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.BuyerUser)
            .WithMany(x => x.BuyerConversations)
            .HasForeignKey(x => x.BuyerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ResponsibleUser)
            .WithMany()
            .HasForeignKey(x => x.ResponsibleUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.LastMessageAt);
        b.HasIndex(x => x.BuyerUserId);
        b.HasIndex(x => x.ResponsibleUserId);

        b.HasIndex(x => x.IsDeleted);
    }
}
