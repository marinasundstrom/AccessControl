using System;
using System.Collections.Generic;

namespace Foobiq.AccessControl.AppService.Domain.Models
{
    public class AccessZone
    {
        public Guid AccessZoneId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<AccessPoint> AccessPoints { get; set; }

        public virtual AccessLog AccessLog { get; set; }
    }
}
