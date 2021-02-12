using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace AppService
{
    public sealed class AccessLogNotifier : IAccessLogNotifier
    {
        private readonly HubConnection hubConnection;
        private Subject<AccessLogEntry> _whenLogAppendedSubject;
        private IDisposable whenLogAppendedSubscription;

        public AccessLogNotifier(HubConnection hubConnection)
        {
            _whenLogAppendedSubject = new Subject<AccessLogEntry>();
            this.hubConnection = hubConnection;
        }

        public IObservable<AccessLogEntry> WhenLogAppended => _whenLogAppendedSubject;

        public async Task StartAsync()
        {
            whenLogAppendedSubscription = hubConnection
                .On<AccessLogEntry>("LogAppended", _whenLogAppendedSubject.OnNext);

            await hubConnection.StartAsync();
        }

        public async Task StopAsync()
        {
            whenLogAppendedSubscription.Dispose();
            await hubConnection.StopAsync();
        }
    }
}
