using System;
using System.Threading;
using System.Threading.Tasks;
using AccessPoint.Application.Services;
using MediatR;

namespace AccessPoint.Application.Lock.Queries
{
    public class GetLockStateQuery : IRequest<LockStateDto>
    {
        public class GetLockStateQueryHandler : IRequestHandler<GetLockStateQuery, LockStateDto>
        {
            private readonly AccessPointState _state;

            public GetLockStateQueryHandler(AccessPointState state)
            {
                _state = state;
            }

            public Task<LockStateDto> Handle(GetLockStateQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new LockStateDto(_state.Locked ?
                                 AccessControl.Contracts.Commands.LockState.Locked
                                 : AccessControl.Contracts.Commands.LockState.Unlocked));
            }
        }
    }
}

