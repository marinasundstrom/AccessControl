using System.ComponentModel.DataAnnotations;
using MediatR;

namespace AppService.Application.Alarm.Commands
{
    public class DisarmCommand : IRequest<AlarmResult>
    {
        [Required]
        public string DeviceId { get; set; }
    }
}
