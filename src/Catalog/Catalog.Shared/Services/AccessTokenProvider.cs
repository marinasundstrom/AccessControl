using System;

using AccessControl.Services;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace AccessControl.Shared.Services;

public class AccessTokenProvider : AccessControl.Services.IAccessTokenProvider
{
    private readonly Microsoft.AspNetCore.Components.WebAssembly.Authentication.IAccessTokenProvider _accessTokenProvider;

    public AccessTokenProvider(Microsoft.AspNetCore.Components.WebAssembly.Authentication.IAccessTokenProvider accessTokenProvider)
    {
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        var results = await _accessTokenProvider.RequestAccessToken(new AccessTokenRequestOptions() { Scopes = new[] { "myapi" } });

        if (results.TryGetToken(out var accessToken))
        {
            return accessToken.Value;
        }

        return null;
    }
}
