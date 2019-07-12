using System;
using Foobiq.AccessControl.AppService.Domain.Models;

namespace Foobiq.AccessControl.AppService.Application.AccessControl
{
    public class AlarmConfiguration : AlarmResult, IAlarmConfiguration
    {
        public TimeSpan AccessTime { get; set; }
        public bool ArmOnClose { get; set; }
        public bool LockOnClose { get; set; }
    }
}
