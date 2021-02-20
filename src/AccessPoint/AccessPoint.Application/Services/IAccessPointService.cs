using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public interface IAccessPointService : IDisposable
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
