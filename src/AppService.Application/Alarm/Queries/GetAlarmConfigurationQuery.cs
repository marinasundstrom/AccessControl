using System.ComponentModel.DataAnnotations;
using MediatR;

namespace AppService.Application.Alarm.Queries
{
    public class GetAlarmConfigurationQuery : IRequest<AlarmConfiguration>
    {
        [Required]
        public string DeviceId { get; set; }
    }
}
