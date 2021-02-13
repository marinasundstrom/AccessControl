using System;
using Newtonsoft.Json;

namespace AppService.Domain.Entities
{
    public class Credential
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Identity Identity { get; set; }
    }
}
