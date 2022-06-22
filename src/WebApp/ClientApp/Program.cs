using System.Globalization;

using AccessControl.Client;
using AccessControl.Client.Authentication;
using AccessControl.Theming;
using AccessControl.IdentityService;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);

    options.UserOptions.NameClaim = "name";
    options.UserOptions.RoleClaim = "role";
});

builder.Services.AddIdentityUI();

builder.Services.AddScoped<AccessControl.Shared.CustomAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("blazor", client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("blazor"));

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddLocalization();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthenticationServices();

builder.Services.AddThemeServices();

builder.Services.AddApiAuthorization()
    .AddAccountClaimsPrincipalFactory<RolesClaimsPrincipalFactory>();

builder.Services.AddHttpClient<AccessControl.Client.IWeatherForecastClient>("blazor")
    .AddTypedClient<IWeatherForecastClient>((http, sp) => new WeatherForecastClient(http));

var app = builder.Build();

await Localize(app.Services);

await app.RunAsync();

static async Task Localize(IServiceProvider serviceProvider)
{
    CultureInfo culture;
    var js = serviceProvider.GetRequiredService<IJSRuntime>();
    var result = await js.InvokeAsync<string>("blazorCulture.get");

    if (result != null)
    {
        culture = new CultureInfo(result);
    }
    else
    {
        culture = new CultureInfo("en-US");
        await js.InvokeVoidAsync("blazorCulture.set", "en-US");
    }

    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}
