using System;
using System.Threading.Tasks;

namespace AccessControl.AppService
{
    public interface IAlarmNotificationClient : IDisposable
    {
        IObservable<AlarmNotification> WhenMessageReceived { get; }

        Task StartAsync();
        Task StopAsync();
    }
}