using System.Globalization;

namespace Foobiq.AccessControl
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}
