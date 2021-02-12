using System;

namespace AppService.Application.Alarm
{
    public interface IAlarmConfiguration
    {
        TimeSpan AccessTime { get; set; }
        bool ArmOnClose { get; set; }
        bool LockOnClose { get; set; }
    }
}