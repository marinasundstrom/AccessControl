using System;
using System.Threading.Tasks;
using AccessControl.AppService.Contracts;

namespace AccessControl.AppService
{
    public interface IAccessLogNotifier
    {
        IObservable<AccessLogEntry> WhenLogAppended { get; }

        Task StartAsync();
        Task StopAsync();
    }
}
