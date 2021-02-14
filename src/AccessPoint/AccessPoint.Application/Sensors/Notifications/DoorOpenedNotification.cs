using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Services;
using AppService;
using MediatR;

namespace AccessPoint.Application.Sensors.Notifications
{
    public class DoorOpenedNotification : INotification
    {
        public class DoorOpenedNotificationHandler : INotificationHandler<DoorOpenedNotification>
        {
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly IBuzzerService _buzzerService;
            private readonly IServiceEventClient _serviceEventClient;

            public DoorOpenedNotificationHandler(
                AccessPointState state,
                ILEDService ledService,
                IBuzzerService buzzerService,
                IServiceEventClient serviceEventClient)
            {
                _state = state;
                _ledService = ledService;
                _buzzerService = buzzerService;
                _serviceEventClient = serviceEventClient;
            }

            public async Task Handle(DoorOpenedNotification notification, CancellationToken cancellationToken)
            {
                if (_state.Armed || _state.Locked)
                {
                    await _serviceEventClient.PublishEvent(new UnauthorizedAccessEvent());

                    await _ledService.ToggleRedLedOn();

                    // In a real-world scenario, this would be going on until manually stopped.

                    await _buzzerService
                        .BuzzAsync(_state.BuzzTime)
                        .ConfigureAwait(false);

                    await _ledService.ToggleAllLedsOff();

                    return;
                }

                await _serviceEventClient.PublishEvent(new AccessControl.Messages.Events.AccessEvent());
            }
        }
    }
}
