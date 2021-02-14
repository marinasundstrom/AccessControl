using System.Threading.Tasks;
using AccessControl.Messages.Events;

namespace AccessPoint.Application.Services
{
    public interface IServiceEventClient
    {
        Task PublishEvent(Event ev);
    }
}
