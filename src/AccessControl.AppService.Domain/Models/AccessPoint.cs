using System;

namespace AccessControl.AppService.Domain.Models
{
    public class AccessPoint
    {
        public Guid AccessPointId { get; set; }

        public string Name { get; set; }

        public string IPAddress { get; set; }

        public TimeSpan AccessTime { get; set; }

        public AccessList AccessList { get; set; }
    }
}
