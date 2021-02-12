using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppService.Application.Services;
using AppService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AppService.Application.Commands
{
    public sealed class GetAlarmStateQueryHandler : IRequestHandler<GetAlarmStateQuery, AlarmResult>
    {
        private readonly DeviceController _deviceController;

        public GetAlarmStateQueryHandler(
            DeviceController deviceController)
        {
            _deviceController = deviceController;
        }

        public async Task<AlarmResult> Handle(GetAlarmStateQuery request, CancellationToken cancellationToken)
        {
            await _deviceController.Arm(request.DeviceId);
            return new AlarmResult {
                AlarmState = (await _deviceController.GetState(request.DeviceId)).AlarmState == AccessControl.Commands.AlarmState.Armed ? AlarmState.Armed : AlarmState.Disarmed
            };
        }
    }
}
