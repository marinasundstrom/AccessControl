using System.Threading.Tasks;
using AppService.Domain.Models;

namespace AppService.Application.Services
{
    public interface IAccessLogNotifier
    {
        Task NotifyLogAppendedAsync(AccessLogEntry accessLogEntry);
    }
}
