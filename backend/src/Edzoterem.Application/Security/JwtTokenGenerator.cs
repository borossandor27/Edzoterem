using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Edzoterem.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Edzoterem.Application.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _settings;

    public JwtTokenGenerator(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public string GenerateToken(Felhasznalo felhasznalo, string nev)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, felhasznalo.Id.ToString()),
            new(ClaimTypes.Role, felhasznalo.Szerepkor.ToString()),
            new("nev", nev),
            new("felhasznalonev", felhasznalo.Felhasznalonev)
        };

        if (felhasznalo.DolgozoId.HasValue)
        {
            claims.Add(new Claim("dolgozoId", felhasznalo.DolgozoId.Value.ToString()));
        }

        if (felhasznalo.TagId.HasValue)
        {
            claims.Add(new Claim("tagId", felhasznalo.TagId.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Kulcs));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Kiado,
            audience: _settings.Kozonseg,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.LejaratPercben),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
