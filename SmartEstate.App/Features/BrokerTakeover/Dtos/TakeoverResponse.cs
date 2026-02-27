using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.BrokerTakeover.Dtos;

public sealed record TakeoverResponse(
    Guid Id,
    Guid ListingId,
    Guid SellerUserId,
    Guid BrokerUserId,
    TakeoverPayer Payer,
    bool IsFeePaid,
    DateTimeOffset? PaidAt,
    TakeoverStatus Status
);
