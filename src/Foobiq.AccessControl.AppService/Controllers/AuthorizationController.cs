using System.Threading.Tasks;
using Foobiq.AccessControl.AppService.Application.AccessControl;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foobiq.AccessControl.AppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuthorizationController : ControllerBase
    {
        private const string DeviceId = "AccessPoint1";
        private readonly IMediator _mediator;

        public AuthorizationController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Authorize")]
        public async Task<AuthorizeCardResult> Authorize(AuthorizeCardCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
