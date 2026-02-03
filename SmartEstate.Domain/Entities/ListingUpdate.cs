using SmartEstate.Domain.Enums;

namespace SmartEstate.Domain.Entities;

public sealed record ListingUpdate(
    string Title,
    string Description,
    PropertyType PropertyType,
    decimal PriceAmount,
    string PriceCurrency,
    double? AreaM2,
    int? Bedrooms,
    int? Bathrooms,
    string? FullAddress,
    string? City,
    string? District,
    string? Ward,
    string? Street,
    double? Lat,
    double? Lng,
    string? VirtualTourUrl
);
