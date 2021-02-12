using System.Security.Claims;

namespace AppService.Application.Services
{
    public interface IJwtTokenService
    {
        string BuildToken(string name, string email);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
