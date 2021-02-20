using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.NotificationHubs;

namespace AppService.Application.Test
{
    public class SendPushNotificationCommand : IRequest
    {
        public SendPushNotificationCommand(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public class SendPushNotificationCommandHandler : IRequestHandler<SendPushNotificationCommand>
        {
            private NotificationHubClient notificationHubClient;

            public SendPushNotificationCommandHandler(
                    NotificationHubClient notificationHubClient)
            {
                this.notificationHubClient = notificationHubClient;
            }

            public async Task<Unit> Handle(SendPushNotificationCommand request, CancellationToken cancellationToken)
            {
                var payload = JsonSerializer.Serialize(new
                {
                    notification = new
                    {
                        title = "Test message",
                        body = request.Text,
                        priority = "10",
                        sound = "default",
                        time_to_live = "600"
                    },
                    data = new
                    {
                        title = "Test message",
                        body = request.Text,
                        url = "https://example.com"
                    }
                });

                await notificationHubClient.SendFcmNativeNotificationAsync(payload, string.Empty);

                return await Unit.Task;
            }
        }
    }
}
