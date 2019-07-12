using System.Threading.Tasks;
using Foobiq.AccessControl.AppService.Domain.Models;

namespace Foobiq.AccessControl.AppService.Application.Services
{
    public interface IAccessLogger
    {
        Task LogAsync(AccessPoint accessPoint, AccessEvent accessEvent, Identity identity, string message);
    }
}
