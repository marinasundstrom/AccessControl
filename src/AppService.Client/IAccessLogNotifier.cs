using System;
using System.Threading.Tasks;

namespace AppService
{
    public interface IAccessLogNotifier
    {
        IObservable<AccessLogEntry> WhenLogAppended { get; }

        Task StartAsync();
        Task StopAsync();
    }
}
