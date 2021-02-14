using System.Threading.Tasks;
using AppService.Application.Rfid.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppService.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class RfidController : Controller
    {
        private readonly IMediator mediator;

        public RfidController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("ReadTag")]
        public async Task<ActionResult<TagDataDto>> ReadTag()
        {
            return await mediator.Send(new ReadTagCommand()
            {
                DeviceId = "AccessPoint1"
            });
        }
    }
}
