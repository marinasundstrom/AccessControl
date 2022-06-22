
using AccessControl.IdentityService.Client;
using AccessControl.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.IdentityService;

public static class ServiceExtensions
{
    public static IServiceCollection AddIdentityUI(this IServiceCollection services)
    {
        services.AddIdentityServiceClients((sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"https://identity.local/");
        }, builder => builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>());

        return services;
    }
}

