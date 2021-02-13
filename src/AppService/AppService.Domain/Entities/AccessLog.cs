using System;
using System.Collections.Generic;

namespace AppService.Domain.Entities
{
    public class AccessLog
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? AccessZoneId { get; set; }

        public AccessZone AccessZone { get; set; }

        public ICollection<AccessLogEntry> Entries { get; set; } = new List<AccessLogEntry>();
    }
}
