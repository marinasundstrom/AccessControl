using Microsoft.AspNetCore.Identity;

namespace Foobiq.AccessPoint.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RefreshToken { get; set; }
    }
}
