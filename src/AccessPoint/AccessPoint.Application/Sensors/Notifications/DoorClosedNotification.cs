using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Alarm.Commands;
using AccessPoint.Application.Lock.Commands;
using AccessPoint.Application.Services;
using AppService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Sensors.Notifications
{
    public class DoorClosedNotification : INotification
    {
        public class DoorClosedNotificationHandler : INotificationHandler<DoorClosedNotification>
        {
            private readonly IMediator _mediator;
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly IRelayControlService _relayControlService;
            private readonly IServiceEventClient _serviceEventClient;
            private readonly ILogger<DoorClosedNotificationHandler> _logger;

            public DoorClosedNotificationHandler(
                IMediator mediator,
                AccessPointState state,
                ILEDService ledService,
                IRelayControlService relayControlService,
                IServiceEventClient serviceEventClient,
                ILogger<DoorClosedNotificationHandler> logger)
            {
                _mediator = mediator;
                _state = state;
                _ledService = ledService;
                _relayControlService = relayControlService;
                _serviceEventClient = serviceEventClient;
                _logger = logger;
            }

            public async Task Handle(DoorClosedNotification notification, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Door closed");

                if (_state.LockWhenShut)
                    {
                        await _mediator.Send(new LockCommand());

                        await _ledService.ToggleAllLedsOff();
                    }

                    if (_state.ArmWhenShut)
                    {
                        await _mediator.Send(new ArmCommand());

                        await _ledService.ToggleAllLedsOff();
                    }

                    _state.Timer?.Dispose();
            }
        }
    }
}
