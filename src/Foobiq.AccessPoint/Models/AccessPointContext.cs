using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Foobiq.AccessPoint.Models
{
    public class AccessPointContext : IdentityDbContext<User>
    {
        public AccessPointContext(DbContextOptions<AccessPointContext> options)
            : base(options)
        {

        }

        public DbSet<Parameter> Settings { get; set; }

        public DbSet<Credential> Credentials { get; set; }

        public DbSet<AccessLog> AccessLogs { get; set; }

        public DbSet<AccessLogEntry> AccessLogEntries { get; set; }

        public DbSet<Identity> Identities { get; set; }

    }
}
