using System.Threading.Tasks;
using AppService.Application.Authorization.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
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
