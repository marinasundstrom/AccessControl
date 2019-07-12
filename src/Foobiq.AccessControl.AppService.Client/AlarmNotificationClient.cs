using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Foobiq.AccessControl.AppService
{
    public sealed class AlarmNotificationClient : IAlarmNotificationClient
    {
        private readonly HubConnection hubConnection;
        private Subject<AlarmNotification> _whenMessageReceivedSubject;
        private IDisposable whenMessageReceivedSubscription;

        public AlarmNotificationClient(HubConnection hubConnection)
        {
            _whenMessageReceivedSubject = new Subject<AlarmNotification>();
            this.hubConnection = hubConnection;
        }

        public IObservable<AlarmNotification> WhenMessageReceived => _whenMessageReceivedSubject.AsObservable().Distinct();

        public void Dispose()
        {
            if(whenMessageReceivedSubscription != null)
            {
               StopAsync().GetAwaiter().GetResult();
            }
        }

        public async Task StartAsync()
        {
            await hubConnection.StartAsync();

            whenMessageReceivedSubscription = hubConnection
                .On<AlarmNotification>("ReceiveAlarmNotification", _whenMessageReceivedSubject.OnNext);
        }

        public async Task StopAsync()
        {
            whenMessageReceivedSubscription.Dispose();
            whenMessageReceivedSubscription = null;
            await hubConnection.StopAsync();
        }
    }
}
