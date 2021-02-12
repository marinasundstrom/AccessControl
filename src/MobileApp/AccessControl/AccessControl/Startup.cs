using AppService;
using AccessControl.Services;
using AccessControl.ViewModels;
using AccessControl.Views;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace AccessControl
{
    public static class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices = null)
        {
            var systemDir = FileSystem.CacheDirectory;
            const string Filename = "AccessControl.appsettings.json";
            Utils.ExtractSaveResource(Filename, systemDir);
            var fullConfig = Path.Combine(systemDir, Filename);

            var host = new HostBuilder()
                            .ConfigureHostConfiguration(c =>
                            {
                                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                                c.AddJsonFile(fullConfig);
                            })
                            .ConfigureServices((c, x) =>
                            {
                                nativeConfigureServices?.Invoke(c, x);
                                ConfigureServices(c, x);
                            })
                            .ConfigureLogging(l => l.AddConsole(o =>
                            {
                                o.DisableColors = true;
                            }))
                            .Build();

            App.ServiceProvider = host.Services;
            ViewModelLocator.ServiceProvider = host.Services;

            return App.ServiceProvider.GetService<App>();
        }


        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            if (ctx.HostingEnvironment.IsDevelopment())
            {
                var world = ctx.Configuration["Hello"];
            }

            var serviceEndpoint = ctx.Configuration["ServiceEndpoint"];

            services.AddSingleton<IResourceContainer, ResourceContainer>();

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;

            async Task<string> RetrieveAuthorizationToken()
            {
                return await SecureStorage.GetAsync("jwt_token");
            }

            services.AddHttpClient<ITokenClient>(client =>
                client.BaseAddress = new Uri(serviceEndpoint))
                .AddTypedClient<ITokenClient>((http, sp) => new TokenClient(http)
                {
                    RetrieveAuthorizationToken = RetrieveAuthorizationToken
                });

            services.AddHttpClient<IRegistrationClient>(client =>
              client.BaseAddress = new Uri(serviceEndpoint))
              .AddTypedClient<IRegistrationClient>((http, sp) => new RegistrationClient(http)
              {
                  RetrieveAuthorizationToken = RetrieveAuthorizationToken
              });

            services.AddHttpClient<IItemsClient>(client =>
              client.BaseAddress = new Uri(serviceEndpoint))
              .AddTypedClient<IItemsClient>((http, sp) => new ItemsClient(http)
              {
                  RetrieveAuthorizationToken = RetrieveAuthorizationToken
              });

            services.AddHttpClient<IAlarmClient>(client =>
              client.BaseAddress = new Uri(serviceEndpoint))
              .AddTypedClient<AlarmClient>();

            services.AddSingleton<IAlarmNotificationClient, AlarmNotificationClient>(sp => 
                new AlarmNotificationClient(
                    new HubConnectionBuilder().WithUrl($"{serviceEndpoint}alarms-notifications-hub", opt =>
                    {
                        //opt.Transports = HttpTransportType.WebSockets;
                        opt.AccessTokenProvider = RetrieveAuthorizationToken;
                    }).Build()));

            services.AddTransient<INavigationService, NavigationService>();

            services.AddTransient<AlarmViewModel>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegistrationViewModel>();

            services.AddTransient<ItemsViewModel>();
            services.AddTransient<ItemDetailViewModel>();
            services.AddTransient<AboutViewModel>();

            services.AddTransient<ItemDetailPage>();

            services.AddTransient<ShellViewModel>();

            services.AddTransient<LoginPage>();
            services.AddTransient<RegistrationPage>();

            services.AddTransient<AppShell>();
            services.AddSingleton<App>();
        }
    }
}
