using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foobiq.AccessControl.Services;
using Foundation;
using UIKit;

namespace Foobiq.AccessControl.iOS.Services
{
    public class PopupService : IPopupService
    {
        public Task<PopupAction> DisplayActionSheetAsync(string title, string message, IEnumerable<PopupAction> actions)
            => CreateAlert(UIAlertControllerStyle.ActionSheet, title, message, actions);

        public Task<PopupAction> DisplayAlertAsync(string title, string message, IEnumerable<PopupAction> actions)
            => CreateAlert(UIAlertControllerStyle.Alert, title, message, actions);

        public Task<PopupAction> DisplayAlertAsync(string title, string message, params PopupAction[] actions)
        {
            return DisplayAlertAsync(title, message, (IEnumerable<PopupAction>)actions);
        }

        private static Task<PopupAction> CreateAlert(UIAlertControllerStyle alertStyle, string title, string message, IEnumerable<PopupAction> actions)
        {
            var tcs = new TaskCompletionSource<PopupAction>();
            var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }
            var alert = UIAlertController.Create(title, message, alertStyle);
            foreach (var action in actions)
            {
                if (action.IsDefault && action.IsCancel)
                    throw new InvalidOperationException();

                var style = action.IsCancel ? UIAlertActionStyle.Cancel
                    : (action.IsDefault ? UIAlertActionStyle.Default : UIAlertActionStyle.Destructive);

                alert.AddAction(UIAlertAction.Create(action.Text, style, a =>
                {
                    try
                    {
                        action?.Command?.Execute(action.CommandParameter);
                        tcs.TrySetResult(action);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                }));
            }
            //if (alert.PopoverPresentationController != null)
            //    alert.PopoverPresentationController.BarButtonItem = myItem;
            vc.PresentViewController(alert, animated: true, completionHandler: null);
            return tcs.Task;
        }
    }
}
