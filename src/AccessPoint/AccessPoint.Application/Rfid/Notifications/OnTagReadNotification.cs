using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessPoint.Application.Alarm.Commands;
using AccessPoint.Application.Authorization.Commands;
using AccessPoint.Application.Services;
using AppService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Rfid.Notifications
{
    public class OnTagReadNotification : INotification
    {
        public CardData TagData { get; private set; }

        public OnTagReadNotification(CardData tagData)
        {
            TagData = tagData;
        }

        public class OnTagReadNotificationHandler : INotificationHandler<OnTagReadNotification>
        {
            private const string DeviceId = "AccessPoint1";

            private readonly IMediator _mediator;
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly IBuzzerService _buzzerService;
            private readonly IAuthorizationClient _authorizationClient;
            private readonly ILogger<OnTagReadNotificationHandler> _logger;

            public OnTagReadNotificationHandler(
                IMediator mediator,
                AccessPointState state,
                ILEDService ledService,
                IBuzzerService buzzerService,
                IAuthorizationClient authorizationClient,
                ILogger<OnTagReadNotificationHandler> logger)
            {
                _mediator = mediator;
                _state = state;
                _ledService = ledService;
                _buzzerService = buzzerService;
                _authorizationClient = authorizationClient;
                _logger = logger;
            }

            public async Task Handle(OnTagReadNotification notification, CancellationToken cancellationToken)
            {
                _logger.LogInformation("RFID Reader read tag:" + string.Join(", ", notification.TagData.UID.Select(x => x.ToString("X"))));

                var ct = new CancellationTokenSource();

                try
                {
                    var blinkTimes = 5;

                    _ = _ledService.Blink(blinkTimes, Color.Blue,
                        callback: async () => await _buzzerService.BuzzAsync(TimeSpan.FromSeconds(1)),
                        cancellationToken: ct.Token);

                    var result = await _mediator.Send(new AuthorizeCommand(DeviceId, notification.TagData.UID));

                    ct.Cancel();

                    if (result.Authorized)
                    {
                        await _mediator.Send(new DisarmCommand());
                    }
                    else
                    {
                        var errorTime = TimeSpan.FromSeconds(10);

                        _ = _ledService.ToggleTimedColor(Color.Red, errorTime, ct.Token);
                        _ = _buzzerService.BuzzAsync(errorTime, ct.Token);
                    }
                }
                catch
                {
                    ct.Cancel();
                }
            }
        }
    }
}
