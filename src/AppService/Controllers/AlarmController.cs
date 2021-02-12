using System.Threading.Tasks;
using AppService.Application.AccessControl;
using AppService.Application.Services;
using AppService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppService.Controllers
{
    [Route("api/[controller]/{deviceId}")]
    [ApiController]
    [Authorize]
    public class AlarmController : ControllerBase
    {
        private const string DeviceId = "AccessPoint1";
        private readonly IMediator _mediator;

        public AlarmController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<AlarmResult>> GetState(string deviceId)
        {
            return Ok(await _mediator.Send(new GetAlarmStateQuery
            {
                DeviceId = deviceId
            }));
        }

        [HttpPost("Arm")]
        public async Task Arm(string deviceId)
        {
            await _mediator.Send(new ArmCommand
            {
                DeviceId = deviceId
            });
        }

        [HttpPost("Disarm")]
        public async Task Disarm(string deviceId)
        {
            await _mediator.Send(new DisarmCommand
            {
                DeviceId = deviceId
            });
        }


        [HttpGet("GetConfiguration")]
        public async Task<ActionResult<AlarmConfiguration>> GetConfiguration(string deviceId)
        {
            return Ok(await _mediator.Send(new GetAlarmConfigurationQuery {
                DeviceId = deviceId
            }));
        }

        [HttpPost("SetConfiguration")]
        public async Task<IActionResult> Configure(SetAlarmConfigurationCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
