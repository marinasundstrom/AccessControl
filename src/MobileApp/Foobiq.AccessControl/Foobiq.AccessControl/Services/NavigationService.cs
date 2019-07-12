using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Foobiq.AccessControl.Views;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace Foobiq.AccessControl.Services
{
    class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PopAsync() => await _serviceProvider.GetService<App>().MainPage.Navigation.PopAsync();

        public async Task PushAsync<TPage>()
            where TPage : Page => await _serviceProvider.GetService<App>().MainPage.Navigation.PushAsync(
                _serviceProvider.GetService<TPage>());

        public void PushLoginPage() => _serviceProvider.GetService<App>().MainPage = new NavigationPage(
                _serviceProvider.GetService<LoginPage>());

        public void PushMainPage<TPage>()
            where TPage : Page => _serviceProvider.GetService<App>().MainPage = _serviceProvider.GetService<TPage>();
    }
}
