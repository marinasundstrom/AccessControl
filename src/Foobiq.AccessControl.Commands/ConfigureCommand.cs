using System;
using System.Collections.Generic;

namespace Foobiq.AccessControl.Commands
{
    public class ConfigureCommand : Command
    {
        public const string ConfigureCommandConstant = "Configure";

        public ConfigureCommand(TimeSpan accessTime, bool lockOnClose, bool armOnClose) : base(ConfigureCommandConstant, new Dictionary<string, object> {
            { nameof(accessTime), accessTime },
            { nameof(lockOnClose), lockOnClose },
            { nameof(armOnClose), armOnClose }
        })
        {
            
        }
    }
}
