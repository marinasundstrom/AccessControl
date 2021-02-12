using System;

namespace AccessControl.AppService.Application.AccessControl
{
    public interface IAlarmConfiguration
    {
        TimeSpan AccessTime { get; set; }
        bool ArmOnClose { get; set; }
        bool LockOnClose { get; set; }
    }
}