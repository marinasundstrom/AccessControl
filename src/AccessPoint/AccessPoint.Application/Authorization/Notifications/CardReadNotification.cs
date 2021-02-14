using System;
using System.Threading;
using System.Threading.Tasks;
using AccessPoint.Application.Alarm.Commands;
using AccessPoint.Application.Services;
using AppService;
using MediatR;

namespace AccessPoint.Application.Authorization.Notifications
{
    public class CardReadNotification : INotification
    {
        public CardData CardDate { get; private set; }

        public CardReadNotification(CardData cardDate)
        {
            CardDate = cardDate;
        }

        public class CardReadNotificationHandler : INotificationHandler<CardReadNotification>
        {
            private const string DeviceId = "AccessPoint1";

            private readonly IMediator _mediator;
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly IBuzzerService _buzzerService;
            private readonly IAuthorizationClient _authorizationClient;

            public CardReadNotificationHandler(
                IMediator mediator,
                AccessPointState state,
                ILEDService ledService,
                IBuzzerService buzzerService,
                IAuthorizationClient authorizationClient)
            {
                _mediator = mediator;
                _state = state;
                _ledService = ledService;
                _buzzerService = buzzerService;
                _authorizationClient = authorizationClient;
            }

            public async Task Handle(CardReadNotification notification, CancellationToken cancellationToken)
            {
                // Console.WriteLine(string.Join(", ", cardData.UID));

                var ct = new CancellationTokenSource();

                try
                {
                    _ = _ledService.BlinkBlue(_buzzerService, ct.Token);

                    var result = await _authorizationClient.AuthorizeAsync(new AuthorizeCardCommand()
                    {
                        DeviceId = DeviceId,
                        CardNo = notification.CardDate.UID,
                        Pin = null
                    });

                    ct.Cancel();

                    if (result.Authorized)
                    {
                        await _mediator.Send(new DisarmCommand());
                    }
                    else
                    {
                        await _ledService.ToggleRedLedOn();

                        await Task.Delay(3000);

                        await _ledService.ToggleAllLedsOff();
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
