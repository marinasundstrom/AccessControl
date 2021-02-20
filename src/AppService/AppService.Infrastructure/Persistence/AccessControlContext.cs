using System;
using AppService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppService.Infrastructure.Persistence
{
    public class AccessControlContext : IdentityDbContext<User>
    {
        public AccessControlContext(DbContextOptions<AccessControlContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccessListMembership>()
                .HasKey(bc => new { bc.IdentityId, bc.AccessListId });

            modelBuilder.Entity<AccessList>()
                .HasMany(al => al.Members)
                .WithMany(i => i.AccessLists)
                .UsingEntity<AccessListMembership>(
                    j => j.HasOne(m => m.Identity).WithMany(x => x.Memberships),
                    j => j.HasOne(m => m.AccessList).WithMany(x => x.Memberships));
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Identity> Identitiets { get; set; }

        public DbSet<Credential> Credentials { get; set; }

        public DbSet<CardCredential> CardCredentials { get; set; }

        public DbSet<AccessZone> AccessZones { get; set; }

        public DbSet<AccessPoint> AccessPoints { get; set; }

        public DbSet<AccessList> AccessLists { get; set; }

        public DbSet<AccessListMembership> AccessListMemberships { get; set; }

        public DbSet<AccessLog> AccessLogs { get; set; }

        public DbSet<AccessLogEntry> AccessLogEntries { get; set; }
    }
}
