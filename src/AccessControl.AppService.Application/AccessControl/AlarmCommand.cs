using System.ComponentModel.DataAnnotations;
using MediatR;

namespace AccessControl.AppService.Application.AccessControl
{
    public class ArmCommand : IRequest<AlarmResult>
    {
        [Required]
        public string DeviceId { get; set; }
    }
}
