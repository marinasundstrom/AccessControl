using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppService.Application.Devices;
using MediatR;

namespace AppService.Application.Alarm.Commands
{
    public class SetAlarmConfigurationCommand : IRequest, IAlarmConfiguration
    {
        [Required]
        public string DeviceId { get; set; }
        public TimeSpan AccessTime { get; set; }
        public bool ArmOnClose { get; set; }
        public bool LockOnClose { get; set; }

        public sealed class SetAlarmConfigurationCommandHandler : IRequestHandler<SetAlarmConfigurationCommand>
        {
            private readonly DeviceController _deviceController;

            public SetAlarmConfigurationCommandHandler(
                DeviceController deviceController)
            {
                _deviceController = deviceController;
            }

            public async Task<Unit> Handle(SetAlarmConfigurationCommand request, CancellationToken cancellationToken)
            {
                await _deviceController.Configure(request.DeviceId, request.AccessTime, request.ArmOnClose, request.LockOnClose);
                return Unit.Value;
            }
        }
    }
}
