using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts.Events;
using AccessPoint.Application.Alarm.Queries;
using AccessPoint.Application.Lock.Commands;
using AccessPoint.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Alarm.Commands
{
    public class DisarmCommand : IRequest<AlarmStateDto>
    {
        public DisarmCommand(bool rex = false)
        {
            Rex = rex;
        }

        public bool Rex { get; }

        public class DisarmCommandHandler : IRequestHandler<DisarmCommand, AlarmStateDto>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<DisarmCommandHandler> _logger;
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
                ILogger<DisarmCommandHandler> logger)
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
                    if (_state.Armed)
                    {
                        _state.Armed = false;

                        _logger.LogInformation("Disarmed");

                        await _serviceEventClient.PublishEvent(new AlarmEvent(AlarmState.Disarmed, request.Rex));

                        var lockState = await _mediator.Send(new UnlockCommand());

                        await _ledService.ToggleGreenLedOn();

                        if (_state.AccessTime != TimeSpan.Zero) // Infinite access time
                        {
                            _state.Timer = new Timer(async _ =>
                            {
                                _state.Timer?.Dispose();
                                _state.Timer = null;

                                await _mediator.Send(new ArmCommand());

                            }, null, (int)_state.AccessTime.TotalMilliseconds, 0);
                        }
                    }

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
