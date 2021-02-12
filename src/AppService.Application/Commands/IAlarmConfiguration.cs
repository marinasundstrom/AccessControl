using System;

namespace AppService.Application.Commands
{
    public interface IAlarmConfiguration
    {
        TimeSpan AccessTime { get; set; }
        bool ArmOnClose { get; set; }
        bool LockOnClose { get; set; }
    }
}