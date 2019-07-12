using System;
using Foobiq.AccessControl.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Foobiq.AccessControl")]
[assembly: ExportEffect(typeof(BorderEffect), "BorderEffect")]
namespace Foobiq.AccessControl.iOS.Effects
{
    public class BorderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                Control.Layer.BorderColor = UIColor.Red.CGColor;
                Control.Layer.BorderWidth = 1;
            }
            catch (Exception)
            {
            }
        }

        protected override void OnDetached()
        {
            try
            {
                Control.Layer.BorderWidth = 0;
            }
            catch (Exception)
            {
            }
        }
    }
}
