namespace AccessControl.Client.Authentication;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}
