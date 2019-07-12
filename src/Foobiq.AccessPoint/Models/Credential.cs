using System;

namespace Foobiq.AccessPoint.Models
{
    public class Credential
    {
        public Guid CredentialId { get; set; }

        public virtual Identity Identity { get; set; }
    }
}
