using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppService.Application.Devices;
using AppService.Application.Services;
using AppService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AppService.Application.Alarm.Commands
{
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
            return new AlarmResult {
                AlarmState = (await _deviceController.GetState(request.DeviceId)).AlarmState == AccessControl.Messages.Commands.AlarmState.Armed ? AlarmState.Armed : AlarmState.Disarmed
            };
        }
    }
}
