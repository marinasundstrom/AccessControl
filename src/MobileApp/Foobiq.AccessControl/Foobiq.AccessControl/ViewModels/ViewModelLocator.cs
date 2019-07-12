using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foobiq.AccessControl.ViewModels
{
    static class ViewModelLocator
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static ItemsViewModel Items => ServiceProvider.GetService<ItemsViewModel>();

        public static AlarmViewModel Alarm => ServiceProvider.GetService<AlarmViewModel>();
    }
}
