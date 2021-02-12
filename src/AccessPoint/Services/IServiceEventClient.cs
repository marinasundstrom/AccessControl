using System.Threading.Tasks;
using AccessControl.Events;

namespace AccessPoint.Services
{
    public interface IServiceEventClient
    {
        Task SendEventAsync(Event ev);
    }
}
