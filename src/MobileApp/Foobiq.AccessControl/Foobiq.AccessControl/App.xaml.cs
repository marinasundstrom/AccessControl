using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Internals;
using Foobiq.AccessControl.Views;
using Xamarin.Essentials;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Foobiq.AccessControl
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; internal set; }
        public static bool IsUserLoggedIn { get; internal set; }

        public App()
        {
            InitializeComponent();

            DependencyResolver.ResolveUsing((t) => ServiceProvider.GetService(t));

            var isLoggedIn = SecureStorage.GetAsync("jwt_token").GetAwaiter().GetResult();

            if (isLoggedIn == null)
            {
                MainPage = new NavigationPage(
                    ServiceProvider.GetService<LoginPage>());
            }
            else
            {
                MainPage = ServiceProvider.GetService<AppShell>();
            }

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var logger = ServiceProvider.GetService<ILogger<App>>();
                logger.LogCritical((Exception)args.ExceptionObject, "Fatal error");              
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
