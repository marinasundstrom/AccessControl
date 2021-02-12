using System;
using System.Threading;
using System.Threading.Tasks;
using AppService.Application.Services;
using MediatR;

namespace AppService.Application.Commands
{
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
