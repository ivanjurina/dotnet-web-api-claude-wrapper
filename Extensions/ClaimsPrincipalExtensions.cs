using System.Security.Claims;

namespace dotnet_webapi_claude_wrapper.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier) 
                ?? throw new InvalidOperationException("User ID not found in claims");
            return int.Parse(userId);
        }
    }
} 