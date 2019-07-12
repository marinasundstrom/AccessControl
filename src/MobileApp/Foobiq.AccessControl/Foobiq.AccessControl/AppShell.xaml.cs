using System;
using System.Collections.Generic;
using Foobiq.AccessControl.ViewModels;
using Xamarin.Forms;

namespace Foobiq.AccessControl
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell(ShellViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}
