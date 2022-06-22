using System;

namespace AppService.Domain.Entities
{
    public class AccessListMembership
    {
        internal AccessListMembership() {}

        public AccessListMembership(Identity identity) 
        {
            Identity = identity;
        }

        public Guid AccessListId { get; set; }

        public AccessList AccessList { get; set; } = null!;

        public Guid IdentityId { get; set; }

        public Identity Identity { get; set; } = null!;
    }
}
