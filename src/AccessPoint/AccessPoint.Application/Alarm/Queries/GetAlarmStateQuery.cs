using System;
using System.Threading;
using System.Threading.Tasks;
using AccessPoint.Application.Services;
using MediatR;

namespace AccessPoint.Application.Alarm.Queries
{
    public class GetAlarmStateQuery : IRequest<AlarmStateDto>
    {
        public class GetAlarmStateQueryHandler : IRequestHandler<GetAlarmStateQuery, AlarmStateDto>
        {
            private readonly AccessPointState _state;

            public GetAlarmStateQueryHandler(AccessPointState state)
            {
                _state = state;
            }

            public Task<AlarmStateDto> Handle(GetAlarmStateQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new AlarmStateDto(_state.Authenticated ?
                                 AccessControl.Messages.Commands.AlarmState.Armed
                                 : AccessControl.Messages.Commands.AlarmState.Disarmed));
            }
        }
    }
}

