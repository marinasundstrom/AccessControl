using System;
using System.Windows.Input;
using Foobiq.AccessControl.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Foobiq.AccessControl.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;

        public ShellViewModel(INavigationService navigationService)
        {
            LogOutCommand = new Command(() =>
            {
                // Clear data

                SecureStorage.Remove("jwt_token");
                SecureStorage.Remove("jwt_refreshtoken");

                _navigationService.PushLoginPage();
            });
            _navigationService = navigationService;
        }

        public ICommand LogOutCommand { get; }
    }
}
