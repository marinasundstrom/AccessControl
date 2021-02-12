using System.Threading.Tasks;
using AppService.Domain.Entities;

namespace AppService.Application.Services
{
    public interface IAccessLogNotifier
    {
        Task NotifyLogAppendedAsync(AccessLogEntry accessLogEntry);
    }
}
