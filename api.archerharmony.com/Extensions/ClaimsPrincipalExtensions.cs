using System.Security.Claims;

namespace api.archerharmony.com.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the username from JWT claims. Tries preferred_username (Keycloak), then email, then name.
    /// Returns "unknown" if no username claim is found.
    /// </summary>
    public static string GetUsername(this ClaimsPrincipal user)
    {
        // Try preferred_username first (Keycloak standard claim)
        var username = user.FindFirst("preferred_username")?.Value;
        if (!string.IsNullOrEmpty(username))
            return username;

        // Fall back to email
        username = user.FindFirst(ClaimTypes.Email)?.Value
                   ?? user.FindFirst("email")?.Value;
        if (!string.IsNullOrEmpty(username))
            return username;

        // Fall back to name
        username = user.FindFirst(ClaimTypes.Name)?.Value
                   ?? user.FindFirst("name")?.Value;
        if (!string.IsNullOrEmpty(username))
            return username;

        // Last resort
        return "unknown";
    }
}