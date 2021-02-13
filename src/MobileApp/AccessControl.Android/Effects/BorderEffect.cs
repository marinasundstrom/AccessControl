using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using AccessControl.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("AccessControl")]
[assembly: ExportEffect(typeof(BorderEffect), "BorderEffect")]
namespace AccessControl.Droid.Effects
{
    public class BorderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var drawable = ContextCompat.GetDrawable(Control.Context, Resource.Drawable.border);
                Control.SetBackground(drawable);
            }
            catch (Exception)
            {
            }
        }

        protected override void OnDetached()
        {
            try
            {
                Control.SetBackground(null);
            }
            catch (Exception)
            {
            }
        }
    }
}
