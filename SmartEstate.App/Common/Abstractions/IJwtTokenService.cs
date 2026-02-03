namespace SmartEstate.App.Common.Abstractions;

public interface IJwtTokenService
{
    string CreateToken(Guid userId, string email, string role);
}
