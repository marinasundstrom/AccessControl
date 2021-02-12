using System;

namespace AppService.Domain.Models
{
    public class IdentityAccessList
    {
        public Guid IdentityId { get; set; }

        public Guid AccessListId { get; set; }

        public Identity Identity { get; set; }

        public AccessList AccessList { get; set; }
    }
}
