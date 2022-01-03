using MassTransit;
using AccessControl.Contracts.Commands;
using MediatR;

namespace AccessPoint.Application.Consumers
{
    public class ConfigureCommandConsumer : IConsumer<ConfigureCommand>
    {
        private readonly IMediator _mediator;

        public ConfigureCommandConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ConfigureCommand> context)
        {
            var message = context.Message;

            var configuration = await _mediator.Send(new Configuration.Commands.SetConfigurationCommand(new Dictionary<string ,object>
            {
                { nameof(message.AccessTime), message.AccessTime },
                { nameof(message.LockOnClose), message.LockOnClose },
                { nameof(message.ArmOnClose), message.ArmOnClose }
            }));

            var response = new ConfigureCommandResponse();

            await context.RespondAsync<ConfigureCommandResponse>(response);
        }
    }
}

