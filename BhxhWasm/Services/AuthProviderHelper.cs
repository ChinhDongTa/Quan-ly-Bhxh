using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BhxhWasm.Services;

public static class AuthProviderHelper {

    public static async Task<string?> GetUserIdAsync(this AuthenticationStateProvider authProvider)
    {
        var auth = await authProvider.GetAuthenticationStateAsync();
        if (auth.User?.Identity?.IsAuthenticated == true)
        {
            return auth.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
        return null;
    }

    public static async Task<bool> HaveRole(this AuthenticationStateProvider authProvider, string roleName)
    {
        var auth = await authProvider.GetAuthenticationStateAsync();
        if (auth.User?.Identity?.IsAuthenticated == true)
        {
            return auth.User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == roleName);
        }
        return false;
    }
}