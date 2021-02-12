using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AppService.Application.Devices;
using AppService.Domain.Entities;
using AppService.Domain.Enums;
using MediatR;

namespace AppService.Application.Alarm.Commands
{
    public class DisarmCommand : IRequest<AlarmResult>
    {
        [Required]
        public string DeviceId { get; set; }

        public sealed class DisarmCommandHandler : IRequestHandler<DisarmCommand, AlarmResult>
        {
            private readonly DeviceController _deviceController;

            public DisarmCommandHandler(
                DeviceController deviceController)
            {
                _deviceController = deviceController;
            }

            public async Task<AlarmResult> Handle(DisarmCommand request, CancellationToken cancellationToken)
            {
                await _deviceController.Disarm(request.DeviceId);
                return new AlarmResult
                {
                    AlarmState = (await _deviceController.GetState(request.DeviceId)).AlarmState == AccessControl.Messages.Commands.AlarmState.Armed ? AlarmState.Armed : AlarmState.Disarmed
                };
            }
        }
    }
}
