using AppService.Contracts;
using AccessControl.Services;
using AccessControl.Validation;
using AccessControl.Views;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AccessControl.ViewModels
{
    public class RegistrationViewModel : ValidationBase
    {
        private readonly IRegistrationClient _registrationClient;
        private readonly INavigationService _navigationService;
        private string email;
        private string password;
        private bool rememberMe;

        public RegistrationViewModel(IRegistrationClient registrationClient, INavigationService navigationService)
        {
            _registrationClient = registrationClient;
            _navigationService = navigationService;

            RegisterCommand = new Command(async () => await ExecuteRegisterCommand());
        }

        private async Task ExecuteRegisterCommand()
        {
            try
            {
                await _registrationClient.RegisterAsync(new AppService.Contracts.RegisterCommand()
                {
                    Email = Email,
                    Password = Password
                });

                await _navigationService.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Command RegisterCommand { get; }


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
