using System.ComponentModel.DataAnnotations;
using MediatR;

namespace AppService.Application.AccessControl
{
    public class GetAlarmConfigurationQuery : IRequest<AlarmConfiguration>
    {
        [Required]
        public string DeviceId { get; set; }
    }
}
