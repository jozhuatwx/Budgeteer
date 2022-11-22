using System.Security.Claims;

namespace Budgeteer.Server.Extensions;

public static class ClaimsExtensions
{
    public static bool TryGetClaim(this ClaimsPrincipal claims, string claimType, out string claim)
    {
        var tryClaim = claims.FindFirstValue(claimType);

        if (tryClaim == null)
        {
            claim = string.Empty;
            return false;
        }

        claim = tryClaim;
        return true;
    }

    public static bool TryGetId(this ClaimsPrincipal claims, out int userId) =>
        int.TryParse(claims.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

    public static bool TryGetName(this ClaimsPrincipal claims, out string name) =>
        claims.TryGetClaim(ClaimTypes.Name, out name);

    public static bool TryGetEmail(this ClaimsPrincipal claims, out string email) =>
        claims.TryGetClaim(ClaimTypes.Email, out email);
}
