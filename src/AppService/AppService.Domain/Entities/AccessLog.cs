﻿using System;
using System.Collections.Generic;

namespace AppService.Domain.Entities
{
    public class AccessLog
    {
        public Guid AccessLogId { get; set; }

        public string Name { get; set; }

        public Guid? AccessZoneId { get; set; }

        public virtual AccessZone AccessZone { get; set; }

        public virtual ICollection<AccessLogEntry> Entries { get; set; }
    }
}