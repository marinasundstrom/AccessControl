using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccessPoint.Application.Configuration.Queries;
using AccessPoint.Application.Models;
using AccessPoint.Application.Services;
using MediatR;

namespace AccessPoint.Application.Configuration.Commands
{
    public class SetConfigurationCommand : IRequest<ConfigurationDto>
    {
        public IDictionary<string, object> Args { get; }

        public SetConfigurationCommand(IDictionary<string, object> args)
        {
            Args = args;
        }

        public class SetConfigurationCommandHandler : IRequestHandler<SetConfigurationCommand, ConfigurationDto>
        {
            private readonly IMediator _mediator;
            private readonly AccessPointContext _accessPointContext;
            private readonly AccessPointState _state;
            private readonly ILEDService _ledService;
            private readonly IRelayControlService _relayControlService;
            private readonly IServiceEventClient _serviceEventClient;

            public SetConfigurationCommandHandler(
                IMediator mediator,
                AccessPointState state,
                AccessPointContext accessPointContext,
                IRelayControlService relayControlService,
                IServiceEventClient serviceEventClient,
                ILEDService ledService)
            {
                _mediator = mediator;
                _accessPointContext = accessPointContext;
                _state = state;
                _relayControlService = relayControlService;
                _serviceEventClient = serviceEventClient;
                _ledService = ledService;
            }

            public async Task<ConfigurationDto> Handle(SetConfigurationCommand request, CancellationToken cancellationToken)
            {
                //Console.WriteLine(JsonConvert.SerializeObject(command.Args));

                _state.AccessTime = TimeSpan.Parse((string)request.Args["accessTime"]);
                _state.LockWhenShut = (bool)request.Args["lockOnClose"];
                _state.ArmWhenShut = (bool)request.Args["armOnClose"];
                await SetParam("accessTime", _state.AccessTime.ToString());
                await SetParam("lockOnClose", _state.LockWhenShut.ToString());
                await SetParam("armOnClose", _state.ArmWhenShut.ToString());

                return await _mediator.Send(new GetConfigurationQuery());
            }

            private async Task SetParam(string key, string value)
            {
                var param = await _accessPointContext.FindAsync<Parameter>(key);
                if (param == null)
                {
                    _accessPointContext.Settings.Add(new Parameter()
                    {
                        Key = key,
                        Value = value
                    });
                }
                else
                {
                    param.Value = value;
                    _accessPointContext.Update(param);
                }
                await _accessPointContext.SaveChangesAsync();
            }
        }
    }
}
