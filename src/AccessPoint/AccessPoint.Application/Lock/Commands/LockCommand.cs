using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Lock.Queries;
using AccessPoint.Application.Services;
using MediatR;

namespace AccessPoint.Application.Lock.Commands
{
    public class LockCommand : IRequest<LockStateDto>
    {
        public class LockCommandHandler : IRequestHandler<LockCommand, LockStateDto>
        {
            private readonly IMediator _mediator;
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly IRelayControlService _relayControlService;
            private readonly IServiceEventClient _serviceEventClient;

            public LockCommandHandler(
                IMediator mediator,
                AccessPointState state,
                IRelayControlService relayControlService,
                IServiceEventClient serviceEventClient,
                ILEDService ledService)
            {
                _mediator = mediator;
                _state = state;
                _relayControlService = relayControlService;
                _serviceEventClient = serviceEventClient;
                _ledService = ledService;
            }

            public async Task<LockStateDto> Handle(LockCommand request, CancellationToken cancellationToken)
            {
                if (!_state.Locked)
                {
                    await _relayControlService.SetRelayStateAsync(_state.LockRelay, true);

                    _state.Locked = true;

                    await _serviceEventClient.PublishEvent(new LockEvent(LockState.Locked));
                }

                return await _mediator.Send(new GetLockStateQuery());
            }
        }
    }
}
