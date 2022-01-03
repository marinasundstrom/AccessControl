using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public interface IServiceEventClient
    {
        Task PublishEvent(object ev);
    }
}
