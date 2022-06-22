using System;
using AppService.Domain.Common;
using Newtonsoft.Json;

namespace AppService.Domain.Entities
{
    public class Credential : AuditableEntity
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Identity Identity { get; set; }
    }
}
