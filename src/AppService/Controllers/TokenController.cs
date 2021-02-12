using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using AppService.Domain.Entities;
using AppService.Application.Login;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using AppService.Application.Services;

namespace AppService.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private IMediator _mediator;

        public TokenController(
            IMediator mediator,
            IConfiguration configuration,
            IJwtTokenService tokenService,
            UserManager<User> userManager)
        {
            _mediator = mediator;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LoginResult>> Auth(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = await _userManager.FindByEmailAsync(email);
            bool validCredentials = await _userManager.CheckPasswordAsync(user, password);

            if (!validCredentials)
            {
                return BadRequest("Username or password is incorrect");
            }

            string name = $"{user.FirstName} {user.LastName}";
            string newJwtToken = GenerateToken(name, email);
            string newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new TokenResult
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<ActionResult<TokenResult>> Refresh([FromForm] string token, [FromForm] string refreshToken)
        {
            ClaimsPrincipal principal = _tokenService.GetPrincipalFromExpiredToken(token);
            string name = GetNameFromClaimsPrincipal(principal);
            string email = GetEmailFromClaimsPrincipal(principal);

            User user = await _userManager.FindByEmailAsync(email);

            string savedRefreshToken = user.RefreshToken;

            if (savedRefreshToken != refreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            string newJwtToken = _tokenService.BuildToken(name, email);
            string newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new TokenResult
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }

        private static string GetNameFromClaimsPrincipal(ClaimsPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            return claimsIdentity.FindFirst(ClaimTypes.Name).Value;
        }

        private static string GetEmailFromClaimsPrincipal(ClaimsPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            return claimsIdentity.FindFirst(ClaimTypes.Email).Value;
        }

        private string GenerateToken(string name, string email) => _tokenService.BuildToken(name, email);

        private string GenerateRefreshToken() => _tokenService.GenerateRefreshToken();
    }
}
