using System;
using System.Threading.Tasks;

namespace AppService
{
    public interface IAlarmNotificationClient : IDisposable
    {
        IObservable<AlarmNotification> WhenMessageReceived { get; }

        Task StartAsync();
        Task StopAsync();
    }
}
