using System.Security.Claims;

namespace TinyExchange.RazorPages.Infrastructure.Extensions;

public static class WebExtensions
{
    public static int GetUserIdFromClaims(this ClaimsPrincipal claimsPrincipal) =>
        int.Parse(claimsPrincipal.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
}