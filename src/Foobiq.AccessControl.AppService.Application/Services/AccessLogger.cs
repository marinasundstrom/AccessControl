using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foobiq.AccessControl.AppService.Domain.Models;
using Foobiq.AccessControl.AppService.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Foobiq.AccessControl.AppService.Application.Services
{
    public sealed class AccessLogger : IAccessLogger
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IAccessLogNotifier _accessLogNotifier;

        public AccessLogger(IServiceScopeFactory serviceScopeFactory,
                    IAccessLogNotifier accessLogNotifier)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this._accessLogNotifier = accessLogNotifier;
        }

        /// <summary>
        /// Adds an entry to the Access Log.
        /// </summary>
        public async Task LogAsync(AccessPoint accessPoint, AccessEvent accessEvent, Identity identity, string message)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetService<AccessControlContext>();
                var logEntry = new AccessLogEntry()
                {
                    AccessPoint = accessPoint != null ? await dataContext.AccessPoints.FindAsync(accessPoint.AccessPointId) : null,
                    Event = accessEvent,
                    Timestamp = DateTime.UtcNow,
                    Identity = identity != null ? await dataContext.Identitiets.FindAsync(identity.IdentityId) : null,
                    Message = message,
                    AccessLog = null
                };
                await dataContext.AccessLogEntries.AddAsync(logEntry);
                await dataContext.SaveChangesAsync();

                await _accessLogNotifier.NotifyLogAppendedAsync(logEntry);
            }
        }
    }
}
