using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Alarm.Commands;
using AccessPoint.Application.Led;
using AccessPoint.Application.Lock.Commands;
using AccessPoint.Application.Services;
using AppService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Sensors.Notifications
{
    public class ButtonPressedNotification : INotification
    {
        public class ButtonPressedNotificationHandler : INotificationHandler<ButtonPressedNotification>
        {
            private readonly IMediator _mediator;
            private readonly ILEDService _ledService;
            private readonly ILogger<ButtonPressedNotificationHandler> _logger;

            public ButtonPressedNotificationHandler(
                IMediator mediator,
                ILEDService ledService,
                ILogger<ButtonPressedNotificationHandler> logger)
            {
                _mediator = mediator;
                _ledService = ledService;
                _logger = logger;
            }

            public async Task Handle(ButtonPressedNotification notification, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Button pressed");

                await _mediator.Send(new DisarmCommand(true));

                /*
                for (int i = 0; i < 30; i++)
                {
                    await _ledService.SetColorAsync(Animation.InterpolateColors(i));

                    await Task.Delay(1000);

                    if (cancellationToken.IsCancellationRequested)
                        return;
                }
                */
            }
        }
    }
}
