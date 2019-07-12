using System.Threading.Tasks;
using Foobiq.AccessControl.AppService.Domain.Models;

namespace Foobiq.AccessControl.AppService.Application.Services
{
    public interface IAccessLogNotifier
    {
        Task NotifyLogAppendedAsync(AccessLogEntry accessLogEntry);
    }
}
