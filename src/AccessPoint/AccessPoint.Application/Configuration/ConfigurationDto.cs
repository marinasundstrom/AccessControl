using System;

namespace AccessPoint.Application.Configuration
{
    public class ConfigurationDto
    {
        public ConfigurationDto(TimeSpan accessTime, bool lockOnClose, bool armOnClose)
        {
            AccessTime = accessTime;
            LockOnClose = lockOnClose;
            ArmOnClose = armOnClose;
        }

        public TimeSpan AccessTime { get; }
        public bool LockOnClose { get; }
        public bool ArmOnClose { get; }
    }
}
