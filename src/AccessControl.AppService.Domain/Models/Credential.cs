using System;
using Newtonsoft.Json;

namespace AccessControl.AppService.Domain.Models
{
    public class Credential
    {
        public Guid CredentialId { get; set; }

        [JsonIgnore]
        public virtual Identity Identity { get; set; }
    }
}
