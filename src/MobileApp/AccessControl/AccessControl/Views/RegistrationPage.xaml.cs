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
    public partial class RegistrationPage : ContentPage
    {
        RegistrationViewModel viewModel => (RegistrationViewModel)BindingContext;

        public RegistrationPage(RegistrationViewModel registrationViewModel)
        {
            BindingContext = registrationViewModel;

            InitializeComponent();
        }
    }
}
