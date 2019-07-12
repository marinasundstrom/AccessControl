using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Foobiq.AccessControl.AppService.Application.AccessControl
{
    public class GetAlarmStateQuery : IRequest<AlarmResult>
    {
        [Required]
        public string DeviceId { get; set; }
    }
}
