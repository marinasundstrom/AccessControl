namespace AccessControl.Services;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}

