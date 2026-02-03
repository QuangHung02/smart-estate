using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.BrokerTakeover.Dtos;

public sealed record TakeoverResponse(
    Guid Id,
    Guid ListingId,
    Guid SellerUserId,
    Guid BrokerUserId,
    TakeoverPayer Payer,
    decimal FeeAmount,
    string FeeCurrency,
    TakeoverStatus Status,
    Guid? PaymentId,
    string? PayUrl
);
