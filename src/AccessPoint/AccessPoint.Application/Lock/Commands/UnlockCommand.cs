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
    public class UnlockCommand : IRequest<LockStateDto>
    {
        public class UnlockCommandHandler : IRequestHandler<UnlockCommand, LockStateDto>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<UnlockCommandHandler> _logger;
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly IRelayControlService _relayControlService;
            private readonly IServiceEventClient _serviceEventClient;

            public UnlockCommandHandler(
                IMediator mediator,
                AccessPointState state,
                IRelayControlService relayControlService,
                IServiceEventClient serviceEventClient,
                ILEDService ledService,
                ILogger<UnlockCommandHandler> logger)
            {
                _mediator = mediator;
                _state = state;
                _relayControlService = relayControlService;
                _serviceEventClient = serviceEventClient;
                _ledService = ledService;
                _logger = logger;
            }

            public async Task<LockStateDto> Handle(UnlockCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (_state.Locked)
                    {
                        await _relayControlService.SetRelayStateAsync(_state.LockRelay, false);

                        _state.Locked = false;

                        _logger.LogInformation("Unlocked");

                        await _serviceEventClient.PublishEvent(new LockEvent(LockState.Unlocked));
                    }

                    return await _mediator.Send(new GetLockStateQuery());
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
