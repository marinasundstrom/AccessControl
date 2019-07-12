using System;
using System.Collections.Generic;

namespace Foobiq.AccessControl.AppService.Domain.Models
{
    public class Identity
    {
        public Guid IdentityId { get; set; }

        public string Name { get; set; }

        public bool IsInactive { get; set; }

        public TimeSpan? ValidFrom { get; set; }

        public TimeSpan? ValidThru { get; set; }

        public ICollection<Credential> Credentials { get; set; }

        public virtual ICollection<IdentityAccessList> IdentityAccessList { get; set; }
    }
}
