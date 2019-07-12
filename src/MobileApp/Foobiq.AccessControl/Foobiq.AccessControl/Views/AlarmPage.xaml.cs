using Foobiq.AccessControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foobiq.AccessControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlarmPage : ContentPage
    {
        AlarmViewModel viewModel => (AlarmViewModel)BindingContext;

        public AlarmPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.InitializeAsync();
        }
    }
}