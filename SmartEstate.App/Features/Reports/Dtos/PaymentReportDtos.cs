namespace SmartEstate.App.Features.Reports.Dtos;

public sealed record PointPurchaseTotalsResponse(
    int Count,
    decimal TotalAmount,
    string Currency
);

