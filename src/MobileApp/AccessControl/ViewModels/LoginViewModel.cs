
using AccessControl.Services;
using AccessControl.Validation;
using AccessControl.Views;
using AppService;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AccessControl.ViewModels
{
    public class LoginViewModel : ValidationBase
    {
        private readonly ITokenClient _tokenClient;
        private readonly IPopupService _popupService;
        private readonly INavigationService _navigationService;
        private string email;
        private string password;
        private bool rememberMe;

        public LoginViewModel(
            ITokenClient tokenClient, 
            IPopupService popupService, 
            INavigationService navigationService)
        {
            _tokenClient = tokenClient;
            _popupService = popupService;
            _navigationService = navigationService;

            LoginCommand = new Command(async () => await ExecuteLoginCommand());
        }

        private async Task ExecuteLoginCommand()
        {
            try
            {
                var response = await _tokenClient.AuthAsync(email, password);

                try
                {
                    await SecureStorage.SetAsync("jwt_token", response.Token);
                    await SecureStorage.SetAsync("jwt_refreshtoken", response.RefreshToken);
                }
                catch (Exception ex)
                {
                    // Possible that device doesn't support secure storage on device.
                }

                _navigationService.PushMainPage<AppShell>();
            }
            catch (Exception ex)
            {
                await _popupService.DisplayAlertAsync("Failed to login", ex.Message,
                    new PopupAction("Cancel", null) { IsDefault = true });

                //await _popupService.DisplayAlertAsync("", "",
                //    new PopupAction("OK", null),
                //    new PopupAction("Cancel", null) { IsCancel = true });
            }
        }

        public Command LoginCommand { get; }

        [Required, EmailAddress]
        public string Email
        {
            get => email;
            set
            {
                ValidateProperty(value);
                SetProperty(ref email, value);
            }
        }

        [Required]
        public string Password
        {
            get => password;
            set
            {
                ValidateProperty(value);
                SetProperty(ref password, value);
            }
        }

        public bool RememberMe
        {
            get => rememberMe;
            set => SetProperty(ref rememberMe, value);
        }

        protected override void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            base.ValidateProperty(value, propertyName);

            OnPropertyChanged("IsSubmitEnabled");
        }

        public bool IsSubmitEnabled
        {
            get
            {
                return !HasErrors;
            }
        }
    }
}
