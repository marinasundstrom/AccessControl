using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AccessPoint.Application.Alarm.Queries
{
    public class GetAlarmStateQuery : IRequest<AlarmStateDto>
    {
        public class GetAlarmStateQueryHandler : IRequestHandler<GetAlarmStateQuery, AlarmStateDto>
        {
            public Task<AlarmStateDto> Handle(GetAlarmStateQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}

