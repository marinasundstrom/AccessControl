using System;

using AccessControl.Shared.Authorization;
using AccessControl.Shared.Services;

using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.Shared;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}

