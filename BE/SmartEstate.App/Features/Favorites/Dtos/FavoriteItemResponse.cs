using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.Favorites.Dtos;

public sealed record FavoriteItemResponse(
    Guid ListingId,
    string Title,
    PropertyType PropertyType,
    decimal PriceAmount,
    string PriceCurrency,
    string? City,
    string? District,
    string? Ward,
    double? Lat,
    double? Lng,
    string? CoverImageUrl,
    DateTimeOffset FavoritedAt
);
