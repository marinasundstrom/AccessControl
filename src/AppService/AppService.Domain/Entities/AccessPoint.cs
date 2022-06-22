using System;
using AppService.Domain.Common;

namespace AppService.Domain.Entities
{
    public class AccessPoint : AuditableEntity
    {
        internal AccessPoint() {}

        public AccessPoint(string name, string ipAddress) 
        {
            Name = name;
            IPAddress = ipAddress;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string IPAddress { get; set; } = null!;

        public TimeSpan AccessTime { get; set; }

        public AccessList AccessList { get; set; } = null!;

        public void AddAccessPoint(AccessList accessList) 
        {
            AccessList = accessList;
        }
    }
}
