using System;
using MassTransit;
using AccessControl.Contracts.Commands;
using MediatR;

namespace AccessPoint.Application.Consumers
{

    public class ReadTagCommandConsumer : IConsumer<ReadTagCommand>
    {
        private readonly IMediator _mediator;

        public ReadTagCommandConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ReadTagCommand> context)
        {
            var message = context.Message;

            var data = await _mediator.Send(new Rfid.Commands.ReadTagCommand());

            var response = new ReadTagCommandResponse
            {
                UID = data.UID
            };

            await context.RespondAsync<ReadTagCommandResponse>(response);
        }
    }
}

