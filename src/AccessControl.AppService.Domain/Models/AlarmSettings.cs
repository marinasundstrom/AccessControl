using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessControl.AppService.Domain.Models
{
    public class AlarmSettings
    {
        public TimeSpan AccessTime { get; set; }
        public bool LockOnClose { get; set; }
        public bool ArmOnClose { get; set; }
    }
}
