using System.Security.Claims;

namespace Hoelterling.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the username from JWT claims. Tries preferred_username (emitted by both Entra and Keycloak),
    /// then email, then name. Returns "unknown" if no username claim is found.
    /// </summary>
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirst("preferred_username")?.Value;
        if (!string.IsNullOrEmpty(username))
            return username;

        username = user.FindFirst(ClaimTypes.Email)?.Value
                   ?? user.FindFirst("email")?.Value;
        if (!string.IsNullOrEmpty(username))
            return username;

        username = user.FindFirst(ClaimTypes.Name)?.Value
                   ?? user.FindFirst("name")?.Value;
        if (!string.IsNullOrEmpty(username))
            return username;

        return "unknown";
    }
}
