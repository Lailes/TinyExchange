using System.Security.Claims;

namespace TinyExchange.RazorPages.Infrastructure.Extensions;

public static class WebExtensions
{
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal) =>
        int.Parse(claimsPrincipal.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
    
    public static async Task<int> WriteMessageAndReturnStatusCodeAsync(this HttpContext context, string message, int statusCode)
    {
        await context.Response.WriteAsync(message);
        return statusCode;
    }
}