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
    public class MotionNotDetectedNotification : INotification
    {
        public class MotionNotDetectedNotificationHandler : INotificationHandler<MotionNotDetectedNotification>
        {
            private readonly ILEDService _ledService;
            private readonly ILogger<MotionNotDetectedNotificationHandler> _logger;

            public MotionNotDetectedNotificationHandler(
                ILEDService ledService,
                ILogger<MotionNotDetectedNotificationHandler> logger)
            {
                _ledService = ledService;
                _logger = logger;
            }

            public async Task Handle(MotionNotDetectedNotification notification, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Motion not detected");

                await _ledService.ToggleAllLedsOff();
            }
        }
    }
}
