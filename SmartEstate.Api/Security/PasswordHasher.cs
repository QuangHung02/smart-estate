using Microsoft.AspNetCore.Identity;
using SmartEstate.App.Common.Abstractions;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Api.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly Microsoft.AspNetCore.Identity.PasswordHasher<User> _hasher = new();

    public string Hash(string password)
    {
        // user param chỉ để salt/versioning, không cần có dữ liệu
        return _hasher.HashPassword(new User(), password);
    }

    public bool Verify(string password, string passwordHash)
    {
        var result = _hasher.VerifyHashedPassword(new User(), passwordHash, password);
        return result is PasswordVerificationResult.Success
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
