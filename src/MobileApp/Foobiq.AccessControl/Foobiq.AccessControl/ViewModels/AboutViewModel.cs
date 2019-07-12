using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace Foobiq.AccessControl.ViewModels
{
    public class AboutViewModel : BindableBase
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}
