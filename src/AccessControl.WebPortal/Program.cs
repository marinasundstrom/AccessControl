﻿using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using AppService;
using AccessControl.WebPortal.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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

            var serviceEndpoint = builder.Configuration["ServiceEndpoint"];

            async Task<string> RetrieveAuthorizationToken(IServiceProvider sp)
            {         
                return await sp.GetRequiredService<TokenAuthenticationStateProvider>().GetTokenAsync();
            }

            builder.Services.AddHttpClient<ITokenClient>(client =>
                client.BaseAddress = new Uri(serviceEndpoint))
                .AddTypedClient<ITokenClient>((http, sp) => new TokenClient(http)
                {
                    RetrieveAuthorizationToken = () => RetrieveAuthorizationToken(sp)
                });

            builder.Services.AddHttpClient<IRegistrationClient>(client =>
                        client.BaseAddress = new Uri(serviceEndpoint))
                        .AddTypedClient<IRegistrationClient>((http, sp) => new RegistrationClient(http)
                        {
                            RetrieveAuthorizationToken = () => RetrieveAuthorizationToken(sp)
                        });

            builder.Services.AddHttpClient<IUserClient>(client =>
                      client.BaseAddress = new Uri(serviceEndpoint))
                      .AddTypedClient<IUserClient>((http, sp) => new UserClient(http)
                      {
                          RetrieveAuthorizationToken = () => RetrieveAuthorizationToken(sp)
                      });

            builder.Services.AddHttpClient<IItemsClient>(client =>
                      client.BaseAddress = new Uri(serviceEndpoint))
                      .AddTypedClient<IItemsClient>((http, sp) => new ItemsClient(http)
                      {
                          RetrieveAuthorizationToken = () => RetrieveAuthorizationToken(sp)
                      });

            builder.Services.AddHttpClient<IAlarmClient>(client =>
                      client.BaseAddress = new Uri(serviceEndpoint))
                      .AddTypedClient<IAlarmClient>((http, sp) => new AlarmClient(http)
                      {
                          RetrieveAuthorizationToken = () => RetrieveAuthorizationToken(sp)
                      });

            builder.Services.AddHttpClient<IAccessLogClient>(client =>
                         client.BaseAddress = new Uri(serviceEndpoint))
                         .AddTypedClient<IAccessLogClient>((http, sp) => new AccessLogClient(http)
                         {
                             RetrieveAuthorizationToken = () => RetrieveAuthorizationToken(sp)
                         });

            builder.Services.AddHttpClient<IIdentitiesClient>(client =>
                         client.BaseAddress = new Uri(serviceEndpoint))
                         .AddTypedClient<IIdentitiesClient>((http, sp) => new IdentitiesClient(http)
                         {
                             RetrieveAuthorizationToken = () => RetrieveAuthorizationToken(sp)
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
