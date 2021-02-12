using System.Threading.Tasks;
using AppService.Domain.Entities;
using AppService.Domain.Enums;

namespace AppService.Application.AccessLog
{
    public interface IAccessLogger
    {
        Task LogAsync(AccessPoint accessPoint, AccessEvent accessEvent, Identity identity, string message);
    }
}
