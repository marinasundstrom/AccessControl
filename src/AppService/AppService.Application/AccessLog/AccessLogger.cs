using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Domain;
using AppService.Domain.Entities;
using AppService.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AppService.Application.AccessLog
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
                var dataContext = scope.ServiceProvider.GetService<IAccessControlContext>();
                var logEntry = new AccessLogEntry()
                {
                    AccessPoint = accessPoint != null ? await dataContext.AccessPoints.FindAsync(accessPoint.Id) : null,
                    Event = accessEvent,
                    Timestamp = DateTime.UtcNow,
                    Identity = identity != null ? await dataContext.Identitiets.FindAsync(identity.Id) : null,
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
