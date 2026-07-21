using ELibraryAPI.Application.Abstractions.Services.Auth;
using ELibraryAPI.Application.Dtos.Auth;
using ELibraryAPI.Application.Options;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ELibraryAPI.Infrastructure.Services.Auth.Token;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;
    public JwtTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateAccessToken(AppUser user, IEnumerable<string> roles, IEnumerable<string> permissions)
    {
        // 1. Key və Credentials hazırlığı
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 2. Claim-lərin (Məlumatların) təyini
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Identity middleware üçün vacibdir
        new Claim("fullName", $"{user.FirstName} {user.LastName}")
    };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        foreach (var permission in permissions)
            claims.Add(new Claim("permission", permission));

        // 3. Token-in yaradılması 
        var expiration = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);

        var securityToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiration, 
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials
        );

        // 4. String formatına çevirib qaytarırıq
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
