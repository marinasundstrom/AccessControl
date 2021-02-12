using System.ComponentModel.DataAnnotations;
using MediatR;

namespace AppService.Application.Alarm.Queries
{
    public class GetAlarmStateQuery : IRequest<AlarmResult>
    {
        [Required]
        public string DeviceId { get; set; }
    }
}
