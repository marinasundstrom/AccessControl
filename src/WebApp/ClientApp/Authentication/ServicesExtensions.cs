namespace AccessControl.Client.Authentication;

public static class ServicesExtensions
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();

        return services;
    }
}
