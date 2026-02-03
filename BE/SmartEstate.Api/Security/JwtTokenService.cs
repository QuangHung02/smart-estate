using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartEstate.App.Common.Abstractions;

namespace SmartEstate.Api.Security;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _opt;

    public JwtTokenService(IOptions<JwtOptions> opt)
    {
        _opt = opt.Value;
    }

    public string CreateToken(Guid userId, string email, string role)
    {
        var claims = new List<Claim>
        {
            // NameIdentifier giúp CurrentUser đọc được
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role),

            // optional: sub
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_opt.ExpiresMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
