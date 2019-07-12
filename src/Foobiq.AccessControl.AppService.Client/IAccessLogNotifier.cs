using System;
using System.Threading.Tasks;
using Foobiq.AccessControl.AppService.Contracts;

namespace Foobiq.AccessControl.AppService
{
    public interface IAccessLogNotifier
    {
        IObservable<AccessLogEntry> WhenLogAppended { get; }

        Task StartAsync();
        Task StopAsync();
    }
}
