
namespace AccessControl.Shared.Authorization;

public interface ICurrentUserService
{
    Task<string?> GetUserId();
    Task<bool> IsUserInRole(string role);
}
