using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MediatR;

namespace AppService.Application.AccessControl
{
    public class SetAlarmConfigurationCommand : IRequest, IAlarmConfiguration
    {
        [Required]
        public string DeviceId { get; set; }
        public TimeSpan AccessTime { get; set; }
        public bool ArmOnClose { get; set; }
        public bool LockOnClose { get; set; }
    }
}
