using AppService.Application.AccessLog;
using AppService.Application.Devices;
using AppService.Application.Services;
using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace AppService.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceExtensions));

        services.AddSingleton<DeviceController>();
        services.AddTransient<IJwtTokenService, JwtTokenService>();

        services.AddSingleton<IAccessLogNotifier, AccessLogNotifier>();
        services.AddSingleton<IAccessLogger, AccessLogger>();

        return services;
    }
}