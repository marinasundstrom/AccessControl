using System;
using AppService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AppService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AppService.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            /*
            services.AddDbContext<AccessControlContext>((sp, options) =>
                options.UseSqlServer(
                    sp.GetRequiredService<IConfiguration>().GetConnectionString("appservice-db")))
               .AddIdentity<User, IdentityRole>()
               .AddEntityFrameworkStores<AccessControlContext>()
               .AddDefaultTokenProviders();
            */

            return services;
        }
    }
}
