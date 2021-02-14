using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Alarm.Queries;
using AccessPoint.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Alarm.Commands
{
    public class DisarmCommand : IRequest<AlarmStateDto>
    {
        public class DisarmCommandHandler : IRequestHandler<DisarmCommand, AlarmStateDto>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<DisarmCommand> _logger;
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly IRelayControlService _relayControlService;
            private readonly IServiceEventClient _serviceEventClient;

            public DisarmCommandHandler(
                IMediator mediator,
                AccessPointState state,
                IRelayControlService relayControlService,
                IServiceEventClient serviceEventClient,
                ILEDService ledService,
                ILogger<DisarmCommand> logger)
            {
                _mediator = mediator;
                _state = state;
                _relayControlService = relayControlService;
                _serviceEventClient = serviceEventClient;
                _ledService = ledService;
                _logger = logger;
            }

            public async Task<AlarmStateDto> Handle(DisarmCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    //if (_unlocked) return;

                    _state.Authenticated = true; //await CheckCardAsync(cardData);

                    if (_state.Authenticated)
                    {
                        _state.Unlocked = true;

                        await _serviceEventClient.SendEventAsync(new AlarmEvent(AccessControl.Messages.Events.AlarmState.Disarmed));
                        await _serviceEventClient.SendEventAsync(new LockEvent(LockState.Unlocked));

                        await _ledService.ToggleGreenLedOn();

                        await _relayControlService.SetRelayStateAsync(_state.LockRelay, true);

                        if (_state.AccessTime != TimeSpan.Zero) // Infinite access time
                        {
                            _state.Timer = new Timer(async _ =>
                            {
                                _state.Timer?.Dispose();
                                _state.Timer = null;

                                _state.Authenticated = false;
                                _state.Unlocked = false;

                                await _relayControlService.SetRelayStateAsync(_state.LockRelay, false);

                                await _serviceEventClient.SendEventAsync(new LockEvent(LockState.Locked));
                                await _serviceEventClient.SendEventAsync(new AlarmEvent(AccessControl.Messages.Events.AlarmState.Armed));

                                await _ledService.ToggleAllLedsOff();
                            }, null, (int)_state.AccessTime.TotalMilliseconds, 0);
                        }
                    }
                    //else
                    //{
                    //    //await ToggleRedLedOn();

                    //    //await _relayControlService.SetRelayStateAsync(LockRelay, false);

                    //    //_timer = new Timer(async _ =>
                    //    //{
                    //    //    _timer?.Dispose();
                    //    //    _timer = null;
                    //    //    _cardOK = false;
                    //    //    _unlocked = false;
                    //    //    await ToggleAllLedsOff();
                    //    //}, null, (int)_buzzTime.TotalMilliseconds, 0);
                    //}

                    return await _mediator.Send(new GetAlarmStateQuery());
                }
                catch (Exception e)
                {
                    _logger.LogError(e, string.Empty);

                    throw;
                }
            }
        }
    }
}
