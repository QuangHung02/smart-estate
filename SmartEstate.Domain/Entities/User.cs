using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class User : AuditableEntity
{
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string DisplayName { get; private set; } = default!;
    public string? Phone { get; private set; }

    public short RoleId { get; private set; }
    public Role Role { get; set; } = default!;

    public bool IsActive { get; private set; } = true;
    public DateTimeOffset? LastLoginAt { get; private set; }

    // Navigation
    public BrokerProfile? BrokerProfile { get; set; }

    public ICollection<Listing> CreatedListings { get; set; } = new List<Listing>();
    public ICollection<Listing> ResponsibleListings { get; set; } = new List<Listing>();

    public ICollection<Conversation> BuyerConversations { get; set; } = new List<Conversation>();
    public ICollection<Message> MessagesSent { get; set; } = new List<Message>();
    public ICollection<UserListingFavorite> FavoriteListings { get; set; } = new List<UserListingFavorite>();


    // -------------------- Factory --------------------
    public static User Create(string email, string displayName, short roleId)
    {
        Guards.AgainstNullOrEmpty(email, "email");
        Guards.AgainstNullOrEmpty(displayName, "displayName");

        return new User
        {
            Email = email.Trim().ToLowerInvariant(),
            DisplayName = displayName.Trim(),
            RoleId = roleId,
            IsActive = true
        };
    }

    // -------------------- Domain methods --------------------
    public void SetPasswordHash(string passwordHash)
    {
        Guards.AgainstNullOrEmpty(passwordHash, "passwordHash");
        PasswordHash = passwordHash;
    }

    public void UpdateDisplayName(string displayName)
    {
        Guards.AgainstNullOrEmpty(displayName, "displayName");
        DisplayName = displayName.Trim();
    }

    public void UpdatePhone(string? phone)
    {
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
    }

    public void SetRoleId(short roleId)
    {
        RoleId = roleId;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void MarkLoggedIn(DateTimeOffset at)
    {
        LastLoginAt = at;
    }

    public void EnsureCanLogin()
    {
        if (IsDeleted) throw new DomainException("User is deleted.");
        if (!IsActive) throw new DomainException("User is inactive.");
    }
}
