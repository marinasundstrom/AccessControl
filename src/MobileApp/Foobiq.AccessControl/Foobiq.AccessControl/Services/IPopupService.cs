using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Foobiq.AccessControl.Services
{
    public class PopupAction
    {
        public PopupAction()
        {

        }

        public PopupAction(string text, ICommand command)
        {
            Text = text;
            Command = command;
        }

        public string Text { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCancel { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
    }

    public interface IPopupService
    {
        Task<PopupAction> DisplayAlertAsync(string title, string message, params PopupAction[] actions);

        Task<PopupAction> DisplayAlertAsync(string title, string message, IEnumerable<PopupAction> actions);

        Task<PopupAction> DisplayActionSheetAsync(string title, string message, IEnumerable<PopupAction> actions);
    }
}
