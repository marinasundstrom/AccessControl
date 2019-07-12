using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazor.Extensions.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Foobiq.AccessControl.WebPortal.Utils
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly LocalStorage _localStorage;

        public TokenAuthenticationStateProvider(LocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string> GetTokenAsync()
            => await _localStorage.GetItem<string>("authToken");

        public async Task SetTokenAsync(string token)
        {
            await _localStorage.SetItem("authToken", token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync();
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ServiceExtensions.ParseClaimsFromJwt(token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
