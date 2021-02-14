using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AppService.Application.Devices;
using MediatR;

namespace AppService.Application.Rfid.Commands
{
    public class ReadTagCommand : IRequest<TagDataDto>
    {
        [Required]
        public string DeviceId { get; set; }

        public class ReadTagCommandHandler : IRequestHandler<ReadTagCommand, TagDataDto>
        {
            private readonly DeviceController _deviceController;

            public ReadTagCommandHandler(DeviceController deviceController)
            {
                _deviceController = deviceController;
            }

            public async Task<TagDataDto> Handle(ReadTagCommand request, CancellationToken cancellationToken)
            {
                var result = await _deviceController.ReadRfidTag(request.DeviceId);
                return new TagDataDto(result.UID);
            }
        }
    }
}
