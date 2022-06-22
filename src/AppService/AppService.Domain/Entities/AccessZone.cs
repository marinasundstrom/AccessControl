using System;
using System.Collections.Generic;
using AppService.Domain.Common;

namespace AppService.Domain.Entities
{
    public class AccessZone : AuditableEntity
    {
        private HashSet<AccessPoint> _accessPoints = new HashSet<AccessPoint>();

        internal AccessZone()
        {
        }

        public AccessZone(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public IReadOnlyCollection<AccessPoint> AccessPoints => _accessPoints;

        public AccessLog AccessLog { get; private set; } = null!;

        public void SetAccessLog(AccessLog accessLog) 
        {
            AccessLog = accessLog;
        }

        public void AddAccessPoint(AccessPoint accessPoint) => _accessPoints.Add(accessPoint);
    }
}
