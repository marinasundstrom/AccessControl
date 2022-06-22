using System;
using System.Collections.Generic;
using AppService.Domain.Common;

namespace AppService.Domain.Entities
{
    public class Identity : AuditableEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsInactive { get; set; }

        public TimeSpan? ValidFrom { get; set; }

        public TimeSpan? ValidThru { get; set; }

        public ICollection<Credential> Credentials { get; set; }

        public ICollection<AccessList> AccessLists { get; set; } = new List<AccessList>();

        public ICollection<AccessListMembership> Memberships { get; set; } = new List<AccessListMembership>();
    }
}
