using System.Threading.Tasks;
using Foobiq.AccessControl.Events;

namespace Foobiq.AccessPoint.Services
{
    public interface IServiceEventClient
    {
        Task SendEventAsync(Event ev);
    }
}
