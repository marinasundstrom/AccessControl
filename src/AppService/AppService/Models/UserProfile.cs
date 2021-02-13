namespace AppService.Domain.Entities
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
