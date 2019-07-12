using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Foobiq.AccessControl.AppService.Application.Services;
using Foobiq.AccessControl.AppService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Foobiq.AccessControl.AppService.Application.AccessControl
{
    public sealed class GetAlarmConfigurationQueryHandler : IRequestHandler<GetAlarmConfigurationQuery, AlarmConfiguration>
    {
        private readonly DeviceController _deviceController;

        public GetAlarmConfigurationQueryHandler(
            DeviceController deviceController)
        {
            _deviceController = deviceController;
        }

        public async Task<AlarmConfiguration> Handle(GetAlarmConfigurationQuery request, CancellationToken cancellationToken)
        {
            var conf = await _deviceController.GetConfiguration(request.DeviceId);
            return new AlarmConfiguration {
                AccessTime = conf.AccessTime,
                ArmOnClose = conf.ArmOnClose,
                LockOnClose = conf.LockOnClose
            };
        }
    }
}
