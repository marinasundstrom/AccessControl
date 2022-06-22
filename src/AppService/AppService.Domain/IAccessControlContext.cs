using AppService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppService.Domain
{
    public interface IAccessControlContext
    {
        DbSet<Item> Items { get; set; }
        DbSet<Identity> Identitiets { get; set; }
        DbSet<Credential> Credentials { get; set; }
        DbSet<CardCredential> CardCredentials { get; set; }
        DbSet<AccessZone> AccessZones { get; set; }
        DbSet<AccessPoint> AccessPoints { get; set; }
        DbSet<AccessList> AccessLists { get; set; }
        DbSet<AccessListMembership> AccessListMemberships { get; set; }
        DbSet<AccessLog> AccessLogs { get; set; }
        DbSet<AccessLogEntry> AccessLogEntries { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}