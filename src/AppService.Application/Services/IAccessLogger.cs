using System.Threading.Tasks;
using AppService.Domain.Entities;

namespace AppService.Application.Services
{
    public interface IAccessLogger
    {
        Task LogAsync(AccessPoint accessPoint, AccessEvent accessEvent, Identity identity, string message);
    }
}
