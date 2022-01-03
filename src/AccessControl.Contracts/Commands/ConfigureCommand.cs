using System;
using System.Collections.Generic;

namespace AccessControl.Contracts.Commands
{
    public class ConfigureCommand
    {
        public ConfigureCommand()
        {

        }

        public ConfigureCommand(TimeSpan accessTime, bool lockOnClose, bool armOnClose)
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
