using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foobiq.AccessControl.AppService.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Foobiq.AccessControl.AppService.Application.Hubs;

namespace Foobiq.AccessControl.AppService.Application.Services
{
    public sealed class AccessLogNotifier : IAccessLogNotifier
    {
        private readonly IHubContext<AccessLogHub> accessLogHubContext;

        public AccessLogNotifier(IHubContext<AccessLogHub> accessLogHubContext)
        {
            this.accessLogHubContext = accessLogHubContext;
        }

        public async Task NotifyLogAppendedAsync(AccessLogEntry accessLogEntry)
        {
            await accessLogHubContext.Clients.All.SendAsync("LogAppended", accessLogEntry);
        }
    }
}
