using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AccessControl.Droid
{
    public static class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://accesscontrol-notification.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=JugKhqZ1pRG+wSBNMjtMq6+x514tHWAkleIdQ4hfr2Y=";
        public const string NotificationHubName = "AccessControl-NotificationHub";
    }
}