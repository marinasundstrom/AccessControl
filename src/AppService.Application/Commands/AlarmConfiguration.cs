using System;
using AppService.Domain.Entities;

namespace AppService.Application.Commands
{
    public class AlarmConfiguration : AlarmResult, IAlarmConfiguration
    {
        public TimeSpan AccessTime { get; set; }
        public bool ArmOnClose { get; set; }
        public bool LockOnClose { get; set; }
    }
}
