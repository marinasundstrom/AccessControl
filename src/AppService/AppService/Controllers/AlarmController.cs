using System.Threading.Tasks;
using AppService.Application.Alarm;
using AppService.Application.Alarm.Commands;
using AppService.Application.Alarm.Queries;
using AppService.Application.Services;
using AppService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppService.Controllers
{
    [Route("api/AccessPoints/{deviceId}/[controller]")]
    [ApiController]
    [Authorize]
    public class AlarmController : ControllerBase
    {
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
            return Ok(await _mediator.Send(new GetAlarmConfigurationQuery
            {
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
