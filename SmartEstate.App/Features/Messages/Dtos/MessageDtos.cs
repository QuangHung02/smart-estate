namespace SmartEstate.App.Features.Messages.Dtos;

public sealed record StartConversationRequest(Guid ListingId, string InitialMessage);

public sealed record SendMessageRequest(string Content);

public sealed record ConversationDto(
    Guid Id,
    Guid ListingId,
    string ListingTitle,
    string ListingImageUrl,
    Guid OtherUserId,
    string OtherUserName,
    string? OtherUserAvatar,
    string? LastMessagePreview,
    DateTimeOffset? LastMessageAt,
    bool IsRead
);

public sealed record MessageDto(
    Guid Id,
    Guid SenderUserId,
    string Content,
    DateTimeOffset SentAt,
    bool IsRead
);