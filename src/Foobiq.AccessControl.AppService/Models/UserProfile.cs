namespace Foobiq.AccessControl.AppService.Domain.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            
        }

        public UserProfile(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
