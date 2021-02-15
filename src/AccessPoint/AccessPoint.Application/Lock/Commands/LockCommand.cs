using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Lock.Queries;
using AccessPoint.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Lock.Commands
{
    public class LockCommand : IRequest<LockStateDto>
    {
        public class LockCommandHandler : IRequestHandler<LockCommand, LockStateDto>
        {
            private readonly IMediator _mediator;
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly ILogger<LockCommandHandler> _logger;
            private readonly IRelayControlService _relayControlService;
            private readonly IServiceEventClient _serviceEventClient;

            public LockCommandHandler(
                IMediator mediator,
                AccessPointState state,
                IRelayControlService relayControlService,
                IServiceEventClient serviceEventClient,
                ILEDService ledService,
                ILogger<LockCommandHandler> logger)
            {
                _mediator = mediator;
                _state = state;
                _relayControlService = relayControlService;
                _serviceEventClient = serviceEventClient;
                _ledService = ledService;
                _logger = logger;
            }

            public async Task<LockStateDto> Handle(LockCommand request, CancellationToken cancellationToken)
            {
                if (!_state.Locked)
                {
                    await _relayControlService.SetRelayStateAsync(_state.LockRelay, true);

                    _state.Locked = true;

                    _logger.LogInformation("Locked");

                    await _serviceEventClient.PublishEvent(new LockEvent(LockState.Locked));
                }

                return await _mediator.Send(new GetLockStateQuery());
            }
        }
    }
}
