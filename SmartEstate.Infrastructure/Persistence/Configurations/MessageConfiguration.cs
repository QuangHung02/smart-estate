using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> b)
    {
        b.ToTable("messages");
        b.HasKey(x => x.Id);

        b.Property(x => x.Content).HasMaxLength(2000).IsRequired();
        b.Property(x => x.AttachmentUrl).HasMaxLength(2000);

        b.HasIndex(x => new { x.ConversationId, x.SentAt });

        b.HasOne(x => x.Conversation)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.SenderUser)
            .WithMany(x => x.MessagesSent)
            .HasForeignKey(x => x.SenderUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.IsDeleted);
    }
}
