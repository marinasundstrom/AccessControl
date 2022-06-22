using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AccessControl.IdentityService;

public static class AuthSchemes
{
    public const string Default =
        JwtBearerDefaults.AuthenticationScheme;
}
