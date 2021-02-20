using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.ViewModels
{
    static class ViewModelLocator
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static ItemsViewModel Items => ServiceProvider.GetService<ItemsViewModel>();

        public static AlarmViewModel Alarm => ServiceProvider.GetService<AlarmViewModel>();
    }
}
