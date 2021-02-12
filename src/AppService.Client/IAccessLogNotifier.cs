using System;
using System.Threading.Tasks;
using AppService.Contracts;

namespace AppService
{
    public interface IAccessLogNotifier
    {
        IObservable<AccessLogEntry> WhenLogAppended { get; }

        Task StartAsync();
        Task StopAsync();
    }
}
