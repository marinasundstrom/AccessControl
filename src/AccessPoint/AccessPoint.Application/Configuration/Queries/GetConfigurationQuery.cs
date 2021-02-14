using System;
using System.Threading;
using System.Threading.Tasks;
using AccessPoint.Application.Services;
using MediatR;

namespace AccessPoint.Application.Configuration.Queries
{
    public class GetConfigurationQuery : IRequest<ConfigurationDto>
    {
        public class GetConfigurationQueryHandler : IRequestHandler<GetConfigurationQuery, ConfigurationDto>
        {
            private readonly AccessPointState _state;

            public GetConfigurationQueryHandler(AccessPointState state)
            {
                _state = state;
            }

            public Task<ConfigurationDto> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new ConfigurationDto(_state.AccessTime, _state.LockWhenShut, _state.ArmWhenShut));
            }
        }
    }
}
