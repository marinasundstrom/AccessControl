namespace AppService.Application.AccessControl
{
    public class AuthorizeCardResult
    {
        public AuthorizeCardResult(bool authorized)
        {
            Authorized = authorized;
        }

        public bool Authorized { get; set; }
    }
}
