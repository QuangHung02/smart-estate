namespace SmartEstate.App.Features.Points.Dtos;

public sealed record PointPackageDto(Guid Id, string Name, int Points, decimal PriceAmount, string PriceCurrency);

public sealed record CreatePointPurchaseRequest(Guid PointPackageId);

public sealed record PointPurchaseInitResponse(Guid PurchaseId, Guid PaymentId, string Provider, string PayUrl);

