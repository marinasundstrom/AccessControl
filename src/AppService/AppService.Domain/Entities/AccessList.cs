using System;
using System.Collections.Generic;

namespace AppService.Domain.Entities
{
    public class AccessList
    {
        public Guid AccessListId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<IdentityAccessList> IdentityAccessList { get; set; }
    }
}
