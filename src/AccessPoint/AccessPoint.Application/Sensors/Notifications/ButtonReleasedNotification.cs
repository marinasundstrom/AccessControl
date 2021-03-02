using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Services;
using AppService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Sensors.Notifications
{
    public class ButtonReleasedNotification : INotification
    {
        public class ButtonReleasedNotificationHandler : INotificationHandler<ButtonReleasedNotification>
        {
            private readonly ILEDService _ledService;
            private readonly ILogger<ButtonReleasedNotificationHandler> _logger;

            public ButtonReleasedNotificationHandler(
                ILEDService ledService,
                ILogger<ButtonReleasedNotificationHandler> logger)
            {
                _ledService = ledService;
                _logger = logger;
            }

            public async Task Handle(ButtonReleasedNotification notification, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Button released");

                //await _ledService.ToggleOff();
            }
        }
    }
}
