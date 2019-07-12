using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Foobiq.AccessControl.AppService.Application.Hubs
{
    [Authorize]
    public class AlarmNotificationsHub : Hub<IAlarmNotificationClient>
    {
        public async Task SendAlarmNotification(AlarmNotification notification)
        {
            await Clients.Caller.ReceiveAlarmNotification(notification);
        }
    }
}
