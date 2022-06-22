
using AccessControl.IdentityService.Application.Common.Interfaces;
using AccessControl.IdentityService.Infrastructure.Persistence;
using AccessControl.IdentityService.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

namespace AccessControl.IdentityService.Infrastructure.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("appservice-db", "IdentityServer") ?? configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
