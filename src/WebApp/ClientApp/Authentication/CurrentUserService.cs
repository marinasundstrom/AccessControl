using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;

namespace AccessControl.Client.Authentication;

public class CurrentUserService : ICurrentUserService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public CurrentUserService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<string?> GetUserId()
    {
        ClaimsPrincipal user = await GetUser();

#if DEBUG
        //Console.WriteLine("Claims: {0}", System.Text.Json.JsonSerializer.Serialize(user.Claims.Select(x => x.Type + " " + x.Value)));
#endif

        var name = user?.FindFirst("sub")?.Value;

#if DEBUG
        //Console.WriteLine("User Id: {0}", name);
#endif

        return name;
    }

    public async Task<bool> IsUserInRole(string role)
    {
        ClaimsPrincipal user = await GetUser();

        var roles = user?.FindAll("role");

        if (roles is null)
        {
            return false;
        }

#if DEBUG
        //Console.WriteLine("Roles: {0}", string.Join(", ", roles.Select(c => c.Value)));
#endif

        return roles.Any(c => c.Value == role);
    }

    private async Task<ClaimsPrincipal> GetUser()
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;
        return user;
    }
}
