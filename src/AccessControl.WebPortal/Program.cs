using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using AccessControl.AppService;
using AccessControl.WebPortal.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AccessControl.WebPortal
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddAuthorizationCore();
            builder.Services.AddTokenAuthenticationStateProvider();

            builder.Services.AddLogging(builder => builder
                 .SetMinimumLevel(LogLevel.Information));

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddSingleton<DomHelpers>();

            builder.Services.AddScoped(sp =>
            {
                var serviceEndpoint = sp.GetRequiredService<NavigationManager>().BaseUri;
                return ClientFactory.CreateTokenClient(serviceEndpoint, sp.GetRequiredService<HttpClient>(), () => Task.FromResult(string.Empty));
            });

            builder.Services.AddScoped(sp =>
            {
                var serviceEndpoint = sp.GetRequiredService<NavigationManager>().BaseUri;
                return ClientFactory.CreateRegistrationClient(serviceEndpoint, sp.GetRequiredService<HttpClient>(), () => Task.FromResult(string.Empty));
            });

            builder.Services.AddScoped(sp =>
            {
                var serviceEndpoint = sp.GetRequiredService<NavigationManager>().BaseUri;
                return ClientFactory.CreateUserClient(serviceEndpoint, sp.GetRequiredService<HttpClient>(), async () => await sp.GetService<ILocalStorageService>().GetItemAsync<string>("authToken"));
            });

            builder.Services.AddScoped(sp =>
            {
                var serviceEndpoint = sp.GetRequiredService<NavigationManager>().BaseUri;
                return ClientFactory.CreateItemsClient(serviceEndpoint, sp.GetRequiredService<HttpClient>(), async () => await sp.GetService<ILocalStorageService>().GetItemAsync<string>("authToken"));
            });

            builder.Services.AddScoped(sp =>
            {
                var serviceEndpoint = sp.GetRequiredService<NavigationManager>().BaseUri;
                return ClientFactory.CreateAlarmClient(serviceEndpoint, sp.GetRequiredService<HttpClient>(), async () => await sp.GetService<ILocalStorageService>().GetItemAsync<string>("authToken"));
            });

            builder.Services.AddScoped(sp =>
            {
                var serviceEndpoint = sp.GetRequiredService<NavigationManager>().BaseUri;
                return ClientFactory.CreateAccessLogClient(serviceEndpoint, sp.GetRequiredService<HttpClient>(), async () => await sp.GetService<ILocalStorageService>().GetItemAsync<string>("authToken"));
            });


            builder.Services.AddScoped(sp =>
            {
                var serviceEndpoint = sp.GetRequiredService<NavigationManager>().BaseUri;
                return ClientFactory.CreateIdentitiesClient(serviceEndpoint, sp.GetRequiredService<HttpClient>(), async () => await sp.GetService<ILocalStorageService>().GetItemAsync<string>("authToken"));
            });


            // Add auth services
            builder.Services.AddAuthorizationCore();
            builder.Services.AddTokenAuthenticationStateProvider();

            builder.Services.AddScoped<IAlarmNotificationClient>(sp =>
            {
                var serviceEndpoint = sp.GetRequiredService<NavigationManager>().BaseUri;
                return new AlarmNotificationClient(
                    new HubConnectionBuilder().WithUrl($"{serviceEndpoint}alarms-notifications-hub").Build());
            });

            builder.Services.AddTransient<IAccessLogNotifier>(sp =>
            {
                var serviceEndpoint = sp.GetRequiredService<NavigationManager>().BaseUri;
                return new AccessLogNotifier(
                    new HubConnectionBuilder().WithUrl($"{serviceEndpoint}accesslog").Build());
            });

            await builder.Build().RunAsync();
        }
    }
}
