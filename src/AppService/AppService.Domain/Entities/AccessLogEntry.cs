using System;
using AppService.Domain.Common;
using AppService.Domain.Enums;

namespace AppService.Domain.Entities
{
    public class AccessLogEntry : AuditableEntity
    {
        internal AccessLogEntry() {}

        public AccessLogEntry(DateTime date, AccessPoint? accessPoint, Identity? identity, AccessEvent @event, string? message) {
            Date = date;
            AccessPoint = accessPoint;
            Identity= identity;
            Event = @event;
            Message = message;
        }

        public Guid Id { get; set; }

        public AccessLog AccessLog { get; set; }  = null!;

        public DateTime Date { get; set; }

        public Guid? AccessPointId { get; set; }

        public AccessPoint? AccessPoint { get; set; }

        public Guid? IdentityId { get; set; }

        public Identity? Identity { get; set; }

        public AccessEvent Event { get; set; }

        public string? Message { get; set; }
    }
}
