using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AppService.Application.AccessLog.Hubs
{
    //[Authorize]
    public class AccessLogHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            /*
            await Clients.All.SendAsync("LogAppended", new AccessLogEntry() {
                AccessLogEntryId = Guid.NewGuid(),
                AccessPointId = Guid.Parse("bab5978e-638d-44da-9999-b478ce3efcd2"),
                Timestamp = DateTime.Now,
                Event = Domain.Enums.AccessEvent.Access,
                Message = "Opened"
            });
            */
        }
    }
}
