using System;
using System.Collections.Generic;
using AccessControl.ViewModels;
using Xamarin.Forms;

namespace AccessControl
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
