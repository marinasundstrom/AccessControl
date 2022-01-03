using MassTransit;
using AccessControl.Contracts.Commands;
using MediatR;

namespace AccessPoint.Application.Consumers
{
    public class ArmCommandConsumer : IConsumer<ArmCommand>
    {
        private readonly IMediator _mediator;

        public ArmCommandConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ArmCommand> context)
        {
            var state = await _mediator.Send(new Alarm.Commands.ArmCommand());

            var response = new ArmCommandResponse(state.AlarmState);

            await context.RespondAsync<ArmCommandResponse>(response);
        }
    }
}

