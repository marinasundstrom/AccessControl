using System.Threading.Tasks;
using AccessControl.AppService.Domain.Models;

namespace AccessControl.AppService.Application.Services
{
    public interface IAccessLogger
    {
        Task LogAsync(AccessPoint accessPoint, AccessEvent accessEvent, Identity identity, string message);
    }
}
