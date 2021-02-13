using System.Threading.Tasks;
using AccessControl.Messages.Events;

namespace AccessPoint.Services
{
    public interface IServiceEventClient
    {
        Task SendEventAsync(Event ev);
    }
}
