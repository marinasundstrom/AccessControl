using MassTransit;
using AccessControl.Contracts.Commands;
using MediatR;

namespace AccessPoint.Application.Consumers
{
    public class GetAlarmStateCommandConsumer : IConsumer<GetAlarmStateCommand>
    {
        private readonly IMediator _mediator;

        public GetAlarmStateCommandConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GetAlarmStateCommand> context)
        {
            var message = context.Message;

            var state = await _mediator.Send(new Alarm.Queries.GetAlarmStateQuery());

            var response = new GetAlarmStateCommandResponse(state.AlarmState);

            await context.RespondAsync<GetAlarmStateCommandResponse>(response);
        }
    }
}

