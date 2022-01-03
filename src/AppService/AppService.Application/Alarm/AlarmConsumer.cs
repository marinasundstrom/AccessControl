using System;
using AccessControl.Contracts.Events;
using AppService.Application.AccessLog;
using AppService.Application.Alarm.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;

namespace AppService.Application.Alarm
{
    public class AlarmConsumer
        : IConsumer<LockEvent>,
        IConsumer<AccessEvent>,
        IConsumer<UnauthorizedAccessEvent>,
        IConsumer<AlarmEvent>
    {
        private readonly NotificationHubClient _notificationHubClient;
        private readonly IHubContext<AlarmNotificationsHub, IAlarmNotificationClient> _hubContext;
        private readonly IAccessLogger _accessLogger;

        public AlarmConsumer(
            NotificationHubClient notificationHubClient,
            IHubContext<AlarmNotificationsHub, IAlarmNotificationClient> hubContext,
            IAccessLogger accessLogger)
        {
            _notificationHubClient = notificationHubClient;
            _hubContext = hubContext;
            _accessLogger = accessLogger;
        }

        public async Task Consume(ConsumeContext<LockEvent> context)
        {
            var message = context.Message;

            await Do(message);
        }

        public async Task Consume(ConsumeContext<AccessEvent> context)
        {
            var message = context.Message;

            await Do(message);
        }

        public async Task Consume(ConsumeContext<UnauthorizedAccessEvent> context)
        {
            var message = context.Message;

            await Do(message);
        }

        public async Task Consume(ConsumeContext<AlarmEvent> context)
        {
            var message = context.Message;

            await Do(message);
        }

        private async Task Do(object ev)
        {
            string deviceId = "test";

            var message = $"{deviceId}: {ev.GetType()}";

            var payload = JsonConvert.SerializeObject(new
            {
                notification = new
                {
                    title = "AccessControl",
                    body = message,
                    priority = "10",
                    sound = "default",
                    time_to_live = "600"
                },
                data = new
                {
                    title = "AccessControl",
                    body = message,
                    url = "https://example.com"
                }
            });

            await _notificationHubClient.SendFcmNativeNotificationAsync(payload, string.Empty);

            await _hubContext.Clients.All.ReceiveAlarmNotification(new AlarmNotification()
            {
                Title = JsonConvert.SerializeObject(ev)
            });

            Domain.Enums.AccessEvent e = Domain.Enums.AccessEvent.Undefined;

            if (ev is AccessControl.Contracts.Events.LockEvent f)
            {
                if (f.LockState == LockState.Locked)
                {
                    e = Domain.Enums.AccessEvent.Locked;
                }
                else
                {
                    e = Domain.Enums.AccessEvent.Unlocked;
                }

            }
            if (ev is AccessControl.Contracts.Events.AlarmEvent g)
            {
                if (g.AlarmState == AlarmState.Armed)
                {
                    e = Domain.Enums.AccessEvent.Armed;
                }
                else
                {
                    e = Domain.Enums.AccessEvent.Disarmed;
                }

            }
            else if (ev is AccessControl.Contracts.Events.AccessEvent)
            {
                e = Domain.Enums.AccessEvent.Access;
            }
            else if (ev is AccessControl.Contracts.Events.UnauthorizedAccessEvent)
            {
                e = Domain.Enums.AccessEvent.UnauthorizedAccess;
            }

            await _accessLogger.LogAsync(null, e, null, string.Empty);
        }
    }
}

