namespace AccessPoint.Application.Authorization.Commands
{
    public class AuthorizationResult
    {
        public AuthorizationResult(bool authorized)
        {
            Authorized = authorized;
        }

        public bool Authorized { get; set; }
    }
}
