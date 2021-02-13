using System;
using AppService.Domain.Enums;

namespace AppService.Domain.Entities
{
    public class AccessLogEntry
    {
        public Guid Id { get; set; }

        public AccessLog AccessLog { get; set; }

        public DateTime Timestamp { get; set; }
        
        public Guid? AccessPointId { get; set; }

        public AccessPoint AccessPoint { get; set; }

        public Guid? IdentityId { get; set; }

        public Identity Identity { get; set; }

        public AccessEvent Event { get; set; }

        public string Message { get; set; }
    }
}
