using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts.Events;
using AccessPoint.Application.Alarm.Commands;
using AccessPoint.Application.Led;
using AccessPoint.Application.Lock.Commands;
using AccessPoint.Application.Services;
using AppService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Sensors.Notifications
{
    public class MotionDetectedNotification : INotification
    {
        public class MotionDetectedNotificationHandler : INotificationHandler<MotionDetectedNotification>
        {
            private readonly ILEDService _ledService;
            private readonly ILogger<MotionDetectedNotificationHandler> _logger;

            public MotionDetectedNotificationHandler(
                ILEDService ledService,
                ILogger<MotionDetectedNotificationHandler> logger)
            {
                _ledService = ledService;
                _logger = logger;
            }

            public async Task Handle(MotionDetectedNotification notification, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Motion detected");

                await _ledService.SetColorAsync(Color.Orange);
            }
        }
    }
}
