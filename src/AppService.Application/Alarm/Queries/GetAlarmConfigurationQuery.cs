using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AppService.Application.Devices;
using MediatR;

namespace AppService.Application.Alarm.Queries
{
    public class GetAlarmConfigurationQuery : IRequest<AlarmConfiguration>
    {
        [Required]
        public string DeviceId { get; set; }

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
                return new AlarmConfiguration
                {
                    AccessTime = conf.AccessTime,
                    ArmOnClose = conf.ArmOnClose,
                    LockOnClose = conf.LockOnClose
                };
            }
        }
    }
}
