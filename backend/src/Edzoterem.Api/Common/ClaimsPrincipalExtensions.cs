using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Edzoterem.Api.Common;

public static class ClaimsPrincipalExtensions
{
    public static int GetFelhasznaloId(this ClaimsPrincipal user) =>
        int.Parse(user.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? throw new InvalidOperationException("Hiányzó 'sub' claim."));

    public static int? GetDolgozoId(this ClaimsPrincipal user)
    {
        var ertek = user.FindFirstValue("dolgozoId");
        return ertek is null ? null : int.Parse(ertek);
    }

    public static int? GetTagId(this ClaimsPrincipal user)
    {
        var ertek = user.FindFirstValue("tagId");
        return ertek is null ? null : int.Parse(ertek);
    }

    public static int GetDolgozoIdRequired(this ClaimsPrincipal user) =>
        user.GetDolgozoId() ?? throw new InvalidOperationException("A felhasználóhoz nem tartozik dolgozó azonosító.");

    public static int GetTagIdRequired(this ClaimsPrincipal user) =>
        user.GetTagId() ?? throw new InvalidOperationException("A felhasználóhoz nem tartozik tag azonosító.");
}
