using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foobiq.AccessControl.Services
{
    public interface INavigationService
    {
        Task PopAsync();
        Task PushAsync<TPage>() where TPage : Page;
        void PushLoginPage();

        void PushMainPage<TPage>() where TPage : Page;
    }
}
