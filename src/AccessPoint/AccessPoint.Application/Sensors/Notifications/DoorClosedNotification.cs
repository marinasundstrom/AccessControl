using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Services;
using AppService;
using MediatR;

namespace AccessPoint.Application.Sensors.Notifications
{
    public class DoorClosedNotification : INotification
    {
        public class DoorClosedNotificationHandler : INotificationHandler<DoorClosedNotification>
        {
            private readonly State _state;
            private readonly ILEDService _ledService;
            private readonly IRelayControlService _relayControlService;
            private readonly IServiceEventClient _serviceEventClient;

            public DoorClosedNotificationHandler(
                State state,
                ILEDService ledService,
                IRelayControlService relayControlService,
                IServiceEventClient serviceEventClient)
            {
                _state = state;
                _ledService = ledService;
                _relayControlService = relayControlService;
                _serviceEventClient = serviceEventClient;
            }

            public async Task Handle(DoorClosedNotification notification, CancellationToken cancellationToken)
            {
                if (_state.Authenticated)
                {
                    if (_state.LockWhenShut)
                    {
                        await _serviceEventClient.SendEventAsync(new LockEvent(LockState.Locked));
                        await _relayControlService.SetRelayStateAsync(_state.LockRelay, false);

                        _state.Unlocked = false;

                        await _ledService.ToggleAllLedsOff();
                    }

                    if (_state.ArmWhenShut)
                    {
                        await _serviceEventClient.SendEventAsync(new AlarmEvent(AccessControl.Messages.Events.AlarmState.Armed));

                        await _ledService.ToggleAllLedsOff();
                    }

                    _state.Timer?.Dispose();
                    _state.Authenticated = false;
                }
            }
        }
    }
}
