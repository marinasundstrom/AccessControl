
using AccessControl.IdentityService.Application.Users.Commands;

using MediatR;

namespace AccessControl.IdentityService.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(CreateUserCommand));

        return services;
    }
}
