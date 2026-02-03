using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.Search.Dtos;

public sealed record SearchItemResponse(
    Guid Id,
    string Title,
    PropertyType PropertyType,
    decimal PriceAmount,
    string PriceCurrency,
    double? AreaM2,
    int? Bedrooms,
    int? Bathrooms,
    string? City,
    string? District,
    string? Ward,
    double? Lat,
    double? Lng,
    string? CoverImageUrl
);
