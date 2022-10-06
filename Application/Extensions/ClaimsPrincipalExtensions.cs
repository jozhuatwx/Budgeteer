using System.Security.Claims;

namespace Playground.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetClaim(this ClaimsPrincipal claims,
        string claimType, out string claim)
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

    public static bool TryGetUserId(this ClaimsPrincipal claims,
        out int userId)
    {
        return int.TryParse(claims.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
    }

    public static bool TryGetUserName(this ClaimsPrincipal claims,
        out string name)
    {
        return claims.TryGetClaim(ClaimTypes.Name, out name);
    }

    public static bool TryGetEmail(this ClaimsPrincipal claims,
        out string email)
    {
        return claims.TryGetClaim(ClaimTypes.Email, out email);
    }
}
