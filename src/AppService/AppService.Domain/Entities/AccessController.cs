using System;
using System.Text;
using AppService.Domain.Common;

namespace AppService.Domain.Entities
{
    public class AccessController : AuditableEntity
    {
        internal AccessController() {}

        public AccessController(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
