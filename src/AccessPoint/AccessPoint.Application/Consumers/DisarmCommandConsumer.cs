using MassTransit;
using AccessControl.Contracts.Commands;
using MediatR;

namespace AccessPoint.Application.Consumers
{
    public class DisarmCommandConsumer : IConsumer<DisarmCommand>
    {
        private readonly IMediator _mediator;

        public DisarmCommandConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<DisarmCommand> context)
        {
            var state = await _mediator.Send(new Alarm.Commands.DisarmCommand());

            var response = new DisarmCommandResponse(state.AlarmState);

            await context.RespondAsync<DisarmCommandResponse>(response);
        }
    }
}

