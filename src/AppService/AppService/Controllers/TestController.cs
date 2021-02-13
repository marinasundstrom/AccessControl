using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Application.Test;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppService.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator mediator;

        public TestController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task SendPushNotification(string text)
        {
            await mediator.Send(new SendPushNotificationCommand(text));
        }
    }
}
