using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.Extensions.Logging;
using Blazor.Extensions.Storage;
using BlazorSignalR;
using Foobiq.AccessControl.AppService;
using Foobiq.AccessControl.WebPortal.Utils;
using Microsoft.AspNetCore.Blazor.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Foobiq.AccessControl.WebPortal
{
    public class Startup
    {
        // TODO: Use HTTPS
        private string serviceEndpoint = "http://localhost:5000/";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder => builder
                 .AddBrowserConsole()
                 .SetMinimumLevel(LogLevel.Trace)
             );

            services.AddStorage();

            services.AddSingleton<DomHelpers>();

            var client = new HttpClient(new WebAssemblyHttpMessageHandler())
            {
                BaseAddress = new Uri(serviceEndpoint)
            };

            services.AddSingleton(sp =>
            {
                return ClientFactory.CreateTokenClient(serviceEndpoint, client, () => Task.FromResult(string.Empty));
            });

            services.AddSingleton(sp => {
                return ClientFactory.CreateRegistrationClient(serviceEndpoint, client, () => Task.FromResult(string.Empty));
            });

            services.AddSingleton(sp =>
            {
                return ClientFactory.CreateUserClient(serviceEndpoint, client, () => sp.GetService<LocalStorage>().GetItem<string>("authToken"));
            });

            services.AddSingleton(sp =>
            {
                return ClientFactory.CreateItemsClient(serviceEndpoint, client, () => sp.GetService<LocalStorage>().GetItem<string>("authToken"));
            });

            services.AddSingleton(sp =>
            {
                return ClientFactory.CreateAlarmClient(serviceEndpoint, client, () => sp.GetService<LocalStorage>().GetItem<string>("authToken"));
            });

            services.AddSingleton(sp =>
            {
                return ClientFactory.CreateAccessLogClient(serviceEndpoint, client, () => sp.GetService<LocalStorage>().GetItem<string>("authToken"));
            });


            services.AddSingleton(sp =>
            {
                return ClientFactory.CreateIdentitiesClient(serviceEndpoint, client, () => sp.GetService<LocalStorage>().GetItem<string>("authToken"));
            });


            // Add auth services
            services.AddAuthorizationCore();
            services.AddTokenAuthenticationStateProvider();

            services.AddTransient<IAlarmNotificationClient>(sp =>
            {
                return new AlarmNotificationClient(
                    new HubConnectionBuilder().WithUrlBlazor($"{serviceEndpoint}alarms-notifications-hub", sp.GetService<IJSRuntime>(),
                                    options: opt =>
                                    {
                                        opt.Transports = HttpTransportType.WebSockets;
                                        opt.AccessTokenProvider = () => sp.GetService<LocalStorage>().GetItem<string>("authToken");
                                    }).Build());
            });

            services.AddTransient<IAccessLogNotifier>(sp =>
            {
                return new AccessLogNotifier(
                    new HubConnectionBuilder().WithUrlBlazor($"{serviceEndpoint}accesslog", sp.GetService<IJSRuntime>(),
                                    options: opt =>
                                    {
                                        opt.Transports = HttpTransportType.WebSockets;
                                        opt.AccessTokenProvider = () => sp.GetService<LocalStorage>().GetItem<string>("authToken");
                                    }).Build());
            });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
