using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.Listings.Dtos;

public sealed record ListingImageDto(Guid Id, string Url, int SortOrder, string? Caption);

public sealed record ListingDetailResponse(
    Guid Id,
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
    string? VirtualTourUrl,
    ModerationStatus ModerationStatus,
    string? ModerationReason,
    decimal? QualityScore,
    ListingLifecycleStatus LifecycleStatus,
    Guid CreatedByUserId,
    Guid ResponsibleUserId,
    string? MaskedPhone,
    IReadOnlyList<ListingImageDto> Images
);
