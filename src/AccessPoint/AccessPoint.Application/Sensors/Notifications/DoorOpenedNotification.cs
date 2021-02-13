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
            private readonly State _state;
            private readonly ILEDService _ledService;
            private readonly IBuzzerService _buzzerService;
            private readonly IServiceEventClient _serviceEventClient;

            public DoorOpenedNotificationHandler(
                State state,
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
                Console.WriteLine(_state.Unlocked);

                if (!_state.Unlocked) //_cardOK
                {
                    _state.Unlocked = false;

                    await _serviceEventClient.SendEventAsync(new UnauthorizedAccessEvent());

                    await _ledService.ToggleRedLedOn();

                    await _buzzerService
                        .BuzzAsync(_state.BuzzTime)
                        .ConfigureAwait(false);

                    await _ledService.ToggleAllLedsOff();

                    return;
                }

                await _serviceEventClient.SendEventAsync(new AccessControl.Messages.Events.AccessEvent());
            }
        }
    }
}
