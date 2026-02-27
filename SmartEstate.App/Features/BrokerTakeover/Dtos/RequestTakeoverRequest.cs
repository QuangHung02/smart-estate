using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.BrokerTakeover.Dtos;

public sealed record RequestTakeoverRequest(
    Guid ListingId,
    Guid BrokerUserId,
    TakeoverPayer Payer,
    string? Note
);
