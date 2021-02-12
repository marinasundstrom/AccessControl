namespace AppService.Domain.Models
{
    public enum AccessEvent
    {
        Undefined = 0,
        Authenticated = 1,
        NotAuthenticated = 2,
        UnauthorizedAccess = 3,
        Unlocked = 4,
        Locked = 5,
        Access = 6,
        Armed = 7,
        Disarmed = 8
    }
}
