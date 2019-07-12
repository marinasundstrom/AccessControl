using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Foobiq.AccessControl.AppService.Application.Registration
{
    public class RegistrationResult
    {
        public bool Succeeded { get; internal set; }
        public IEnumerable<IdentityError> Errors { get; internal set; }
    }
}
