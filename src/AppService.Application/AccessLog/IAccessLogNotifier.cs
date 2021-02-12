using System.Threading.Tasks;
using AppService.Domain.Entities;

namespace AppService.Application.AccessLog
{
    public interface IAccessLogNotifier
    {
        Task NotifyLogAppendedAsync(AccessLogEntry accessLogEntry);
    }
}
