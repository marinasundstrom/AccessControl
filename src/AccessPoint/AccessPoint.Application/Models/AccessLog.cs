using System;
using System.Collections.Generic;

namespace AccessPoint.Application.Models
{
    public class AccessLog
    {
        public Guid AccessLogId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<AccessLogEntry> Entries { get; set; }
    }
}
