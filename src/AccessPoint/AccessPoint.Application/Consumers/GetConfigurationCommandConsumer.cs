using MassTransit;
using AccessControl.Contracts.Commands;
using MediatR;

namespace AccessPoint.Application.Consumers
{
    public class GetConfigurationCommandConsumer : IConsumer<GetConfigurationCommand>
    {
        private readonly IMediator _mediator;

        public GetConfigurationCommandConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GetConfigurationCommand> context)
        {
            var message = context.Message;

            var configuration = await _mediator.Send(new Configuration.Queries.GetConfigurationQuery());

            var response = new GetConfigurationCommandResponse(configuration.AccessTime, configuration.LockOnClose, configuration.ArmOnClose);

            await context.RespondAsync<GetConfigurationCommandResponse>(response);
        }
    }
}

