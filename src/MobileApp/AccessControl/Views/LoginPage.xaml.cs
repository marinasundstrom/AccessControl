using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccessControl.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccessControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly IServiceProvider _serviceProvider;

        LoginViewModel viewModel => (LoginViewModel)BindingContext;

        public LoginPage(LoginViewModel loginViewModel, IServiceProvider serviceProvider)
        {
            BindingContext = loginViewModel;
            _serviceProvider = serviceProvider;

            InitializeComponent();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_serviceProvider.GetService<RegistrationPage>());
        }
    }
}
