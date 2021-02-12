using AppService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AppService.Persistence
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

            modelBuilder.Entity<IdentityAccessList>()
                .HasKey(bc => new { bc.IdentityId, bc.AccessListId });
            modelBuilder.Entity<IdentityAccessList>()
                .HasOne(bc => bc.Identity)
                .WithMany(b => b.IdentityAccessList)
                .HasForeignKey(bc => bc.IdentityId);
            modelBuilder.Entity<IdentityAccessList>()
                .HasOne(bc => bc.AccessList)
                .WithMany(c => c.IdentityAccessList)
                .HasForeignKey(bc => bc.AccessListId);
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Identity> Identitiets { get; set; }

        public DbSet<Credential> Credentials { get; set; }

        public DbSet<CardCredential> CardCredentials { get; set; }

        public DbSet<AccessZone> AccessZones { get; set; }

        public DbSet<AccessPoint> AccessPoints { get; set; }

        public DbSet<AccessList> AccessLists { get; set; }

        public DbSet<AccessLog> AccessLogs { get; set; }

        public DbSet<AccessLogEntry> AccessLogEntries { get; set; }
    }
}
