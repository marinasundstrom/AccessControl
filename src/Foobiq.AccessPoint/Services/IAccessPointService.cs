using System;
using System.Threading;
using System.Threading.Tasks;

namespace Foobiq.AccessPoint.Services
{
    public interface IAccessPointService : IDisposable
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}