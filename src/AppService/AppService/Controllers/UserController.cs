using System.Threading.Tasks;
using AppService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppService.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<UserProfile>> GetProfile()
        {
            var user = await _userManager.GetUserAsync(ControllerContext.HttpContext.User);
            return Ok(new UserProfile($"{user.FirstName} {user.LastName}"));
        }
    }
}
