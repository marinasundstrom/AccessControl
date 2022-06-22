using AccessControl.IdentityService.Application.Common.Interfaces;
using AccessControl.IdentityService.Services;

namespace AccessControl.IdentityService;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IEventPublisher, EventPublisher>();

        return services;
    }
}
