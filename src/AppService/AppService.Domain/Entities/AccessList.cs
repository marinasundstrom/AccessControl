using System;
using System.Collections.Generic;

namespace AppService.Domain.Entities
{
    public class AccessList
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Identity> Members { get; set; } = new List<Identity>();

        public ICollection<AccessListMembership> Memberships { get; set; } = new List<AccessListMembership>();
    }
}
