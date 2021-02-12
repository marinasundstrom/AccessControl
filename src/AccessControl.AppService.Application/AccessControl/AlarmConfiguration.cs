using System;
using AccessControl.AppService.Domain.Models;

namespace AccessControl.AppService.Application.AccessControl
{
    public class AlarmConfiguration : AlarmResult, IAlarmConfiguration
    {
        public TimeSpan AccessTime { get; set; }
        public bool ArmOnClose { get; set; }
        public bool LockOnClose { get; set; }
    }
}
