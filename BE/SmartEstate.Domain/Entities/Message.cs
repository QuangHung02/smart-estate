using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class Message : AuditableEntity
{
    public Guid ConversationId { get; set; }
    public Guid SenderUserId { get; set; }

    public string Content { get; set; } = default!;
    public DateTimeOffset SentAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsRead { get; set; }
    public DateTimeOffset? ReadAt { get; set; }

    // optional attachments
    public string? AttachmentUrl { get; set; }

    // Navigation
    public Conversation Conversation { get; set; } = default!;
    public User SenderUser { get; set; } = default!;
}
