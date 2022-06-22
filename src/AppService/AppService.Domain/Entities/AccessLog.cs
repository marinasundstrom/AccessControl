using System;
using System.Collections.Generic;
using AppService.Domain.Common;

namespace AppService.Domain.Entities
{
    public class AccessLog : AuditableEntity
    {
        private HashSet<AccessLogEntry> _entries = new HashSet<AccessLogEntry>();

        internal AccessLog() {}

        public AccessLog(string name) 
        {
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public Guid AccessZoneId { get; set; }

        public AccessZone AccessZone { get; set; } = null!;

        public IReadOnlyCollection<AccessLogEntry> Entries => _entries;

        public AccessLogEntry AddEntry(AccessLogEntry accessLogEntry)
        {
            //var accessLogEntry = new AccessLogEntry);
            _entries.Add(accessLogEntry);
            return accessLogEntry;
        }
    }
}
