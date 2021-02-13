using System;

namespace AppService.Domain.Entities
{
    public class AccessPoint
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IPAddress { get; set; }

        public TimeSpan AccessTime { get; set; }

        public AccessList AccessList { get; set; }
    }
}
