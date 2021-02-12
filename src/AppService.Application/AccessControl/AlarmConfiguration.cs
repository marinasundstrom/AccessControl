using System;
using AppService.Domain.Models;

namespace AppService.Application.AccessControl
{
    public class AlarmConfiguration : AlarmResult, IAlarmConfiguration
    {
        public TimeSpan AccessTime { get; set; }
        public bool ArmOnClose { get; set; }
        public bool LockOnClose { get; set; }
    }
}
