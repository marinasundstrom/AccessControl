using AppService.Application.Hubs;
using AccessControl.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Devices;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppService.Application.Services
{
    public class AlarmService : IHostedService
    {
        private readonly EventHubClient eventHubClient;
        private readonly NotificationHubClient notificationHubClient;
        private readonly IHubContext<AlarmNotificationsHub, IAlarmNotificationClient> hubContext;
        private readonly IAccessLogger _accessLogger;

        public AlarmService(
            EventHubClient eventHubClient,
            NotificationHubClient notificationHubClient,
            IHubContext<AlarmNotificationsHub, IAlarmNotificationClient> hubContext,
            IAccessLogger accessLogger)
        {
            this.eventHubClient = eventHubClient;
            this.notificationHubClient = notificationHubClient;
            this.hubContext = hubContext;
            _accessLogger = accessLogger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var info = await eventHubClient.GetRuntimeInformationAsync();
            var partitions = info.PartitionIds;
            var cts = new CancellationTokenSource();

            var tasks = partitions.Select(partition => ReceiveMessagesFromDeviceAsync(partition, cts.Token));
            tasks.ToArray();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken cancellationToken)
        {
            var eventHubReceiver = eventHubClient.CreateReceiver(PartitionReceiver.DefaultConsumerGroupName, partition, EventPosition.FromEnd());
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) break;
                var eventData = await eventHubReceiver.ReceiveAsync(1);
                if (eventData == null) continue;

                foreach (var d in eventData)
                {
                    var data = Encoding.UTF8.GetString(d.Body.ToArray());
                    Event ev = JsonConvert.DeserializeObject<Event>(data); ;
                    switch(ev.EventName)
                    {
                        case LockEvent.EventNameConstant:
                           ev = JsonConvert.DeserializeObject<LockEvent>(data);
                            break;

                        case AccessEvent.EventNameConstant:
                            ev = JsonConvert.DeserializeObject<AccessEvent>(data);
                            break;

                        case UnauthorizedAccessEvent.EventNameConstant:
                            ev = JsonConvert.DeserializeObject<UnauthorizedAccessEvent>(data);
                            break;

                        case AlarmEvent.EventNameConstant:
                            ev = JsonConvert.DeserializeObject<AlarmEvent>(data);
                            break;
                    }

                    string deviceId = d.SystemProperties["iothub-connection-device-id"].ToString();
                    await notificationHubClient.SendFcmNativeNotificationAsync("{\"data\":{\"message\":\"" + $"{deviceId}: {ev.EventName}" + "\"}}", string.Empty);

                    await hubContext.Clients.All.ReceiveAlarmNotification(new AlarmNotification()
                    {
                        Title = data
                    });

                    Domain.Models.AccessEvent e = Domain.Models.AccessEvent.Undefined;

                    if(ev is AccessControl.Events.LockEvent f)
                    {
                        if(f.LockState == LockState.Locked)
                        {
                            e = Domain.Models.AccessEvent.Locked;
                        } else
                        {
                            e = Domain.Models.AccessEvent.Unlocked;
                        }
                       
                    }
                    if (ev is AccessControl.Events.AlarmEvent g)
                    {
                        if (g.AlarmState == AlarmState.Armed)
                        {
                            e = Domain.Models.AccessEvent.Armed;
                        }
                        else
                        {
                            e = Domain.Models.AccessEvent.Disarmed;
                        }

                    }
                    else if (ev is AccessControl.Events.AccessEvent)
                    {
                        e = Domain.Models.AccessEvent.Access;
                    }
                    else if (ev is AccessControl.Events.UnauthorizedAccessEvent)
                    {
                        e = Domain.Models.AccessEvent.UnauthorizedAccess;
                    }

                    await _accessLogger.LogAsync(null, e, null, string.Empty);
                }
            }
        }
    }
}
