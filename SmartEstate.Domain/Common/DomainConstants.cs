namespace SmartEstate.Domain.Common;

public static class DomainConstants
{
    public const int MaxListingTitleLength = 200;
    public const int MaxListingDescriptionLength = 5000;
    public const int MaxImagesPerListing = 50;

    public const int MaxMessageLength = 2000;

    public const decimal MinPrice = 0;
    public const double MinArea = 0;

    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;
}
