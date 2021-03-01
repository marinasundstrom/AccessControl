using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Alarm.Queries;
using AccessPoint.Application.Lock.Commands;
using AccessPoint.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Alarm.Commands
{
    public class ArmCommand : IRequest<AlarmStateDto>
    {
        public class ArmCommandHandler : IRequestHandler<ArmCommand, AlarmStateDto>
        {
            private readonly IMediator _mediator;
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly ILogger<ArmCommandHandler> _logger;
            private readonly IRelayControlService _relayControlService;
            private readonly IServiceEventClient _serviceEventClient;

            public ArmCommandHandler(
                IMediator mediator,
                AccessPointState state,
                IRelayControlService relayControlService,
                IServiceEventClient serviceEventClient,
                ILEDService ledService,
                ILogger<ArmCommandHandler> logger)
            {
                _mediator = mediator;
                _state = state;
                _relayControlService = relayControlService;
                _serviceEventClient = serviceEventClient;
                _ledService = ledService;
                _logger = logger;
            }

            public async Task<AlarmStateDto> Handle(ArmCommand request, CancellationToken cancellationToken)
            {
                if (!_state.Armed)
                {
                    var lockState = await _mediator.Send(new LockCommand());

                    _state.Armed = true;

                    await _ledService.SetColorAsync(Color.Purple);

                    _logger.LogInformation("Armed");

                    await _serviceEventClient.PublishEvent(new AlarmEvent(AlarmState.Armed));

                    await Task.Delay(2000);

                    await _ledService.ToggleOff();
                }

                return await _mediator.Send(new GetAlarmStateQuery());
            }
        }
    }
}
