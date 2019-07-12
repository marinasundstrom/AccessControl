using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Foobiq.AccessControl.AppService.Domain.Models;
using Foobiq.AccessControl.AppService.Application.Services;
using MediatR;
using Foobiq.AccessControl.AppService.Application.Registration;

namespace Foobiq.AccessControl.AppService.Controllers
{

    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Registration method to create new Identity users
        [HttpPost]
        [Route("Registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<RegistrationResult>> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return StatusCode(500, result.Errors);
            }

            return result;
        }
    }
}
