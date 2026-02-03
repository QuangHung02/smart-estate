using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.Listings.Dtos;

public sealed record UpdateListingRequest(
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
