using System;
using System.Collections.Generic;

namespace AppService.Domain.Entities
{
    public class AccessZone
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<AccessPoint> AccessPoints { get; set; } = new List<AccessPoint>();

        public AccessLog AccessLog { get; set; }
    }
}
