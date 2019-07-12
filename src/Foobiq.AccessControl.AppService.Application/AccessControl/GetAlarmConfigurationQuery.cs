using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Foobiq.AccessControl.AppService.Application.AccessControl
{
    public class GetAlarmConfigurationQuery : IRequest<AlarmConfiguration>
    {
        [Required]
        public string DeviceId { get; set; }
    }
}
