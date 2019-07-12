using System;
using System.Collections.Generic;

namespace Foobiq.AccessControl.AppService.Domain.Models
{
    public class AccessList
    {
        public Guid AccessListId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<IdentityAccessList> IdentityAccessList { get; set; }
    }
}
