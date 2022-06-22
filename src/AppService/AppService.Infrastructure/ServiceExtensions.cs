using System;
using AppService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AppService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using AppService.Domain;
using System.IdentityModel.Tokens.Jwt;
using AppService.Infrastructure.Persistence.Interceptors;
using AppService.Application.Services;
using AppService.Infrastructure.Services;

namespace AppService.Infrastructure;

public static class ServiceExtensions
{
    private const string ConnectionStringKey = "appservice-db";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringKey, "AccessControl");

        services.AddDbContext<AccessControlContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
        });

        services.AddScoped<IAccessControlContext>(sp => sp.GetRequiredService<AccessControlContext>());

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        //AddIdentity(services);

        services.AddScoped<IDateTimeService, DateTimeService>();

        services.AddScoped<IDomainEventService, DomainEventService>();

        return services;
    }

    /*
    private static void AddIdentity(IServiceCollection services)
    {
        services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<User, ApplicationDbContext>(opt =>
            {
                // Is this necessary with a profile?

                opt.IdentityResources["openid"].UserClaims.Add("role");
                opt.ApiResources.Single().UserClaims.Add("role");
            });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");
    }
    */
}
