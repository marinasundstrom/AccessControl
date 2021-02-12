using System.Threading.Tasks;
using AccessControl.AppService.Domain.Models;

namespace AccessControl.AppService.Application.Services
{
    public interface IAccessLogNotifier
    {
        Task NotifyLogAppendedAsync(AccessLogEntry accessLogEntry);
    }
}
