using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class UserListingFavorite : AuditableEntity
{
    public Guid UserId { get; set; }
    public Guid ListingId { get; set; }

    // Navigation
    public User User { get; set; } = default!;
    public Listing Listing { get; set; } = default!;

    public static UserListingFavorite Create(Guid userId, Guid listingId)
    {
        Guards.AgainstDefaultGuid(userId, "userId");
        Guards.AgainstDefaultGuid(listingId, "listingId");

        return new UserListingFavorite
        {
            UserId = userId,
            ListingId = listingId
        };
    }
}
