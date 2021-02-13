using System;

namespace AccessPoint.Application.Models
{
    public class Credential
    {
        public Guid CredentialId { get; set; }

        public virtual Identity Identity { get; set; }
    }
}
