using Microsoft.AspNetCore.Identity;

namespace Foobiq.AccessControl.AppService.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RefreshToken { get; set; }
    }
}
