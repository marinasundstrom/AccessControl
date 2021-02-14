using System;
using System.Threading;
using System.Threading.Tasks;
using AccessPoint.Application.Services;
using MediatR;

namespace AccessPoint.Application.Rfid.Commands
{
    public class ReadTagCommand : IRequest<CardData>
    {
        public class ReadCardCommandHandler : IRequestHandler<ReadTagCommand, CardData>
        {
            private IRfidReader rfidReader;

            public ReadCardCommandHandler(
                    IRfidReader rfidReader)
            {
                this.rfidReader = rfidReader;
            }

            public async Task<CardData> Handle(ReadTagCommand request, CancellationToken cancellationToken)
            {
                var tag = await rfidReader.ReadCardUniqueIdAsync();
                return new CardData(tag.UID);
            }
        }
    }
}
