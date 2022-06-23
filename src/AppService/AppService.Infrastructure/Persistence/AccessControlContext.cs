using System;
using AppService.Application.Services;
using AppService.Domain;
using AppService.Domain.Common;
using AppService.Domain.Entities;
using AppService.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppService.Infrastructure.Persistence
{
    public class AccessControlContext : IdentityDbContext<User>, IAccessControlContext
    {
        private readonly IDomainEventService _domainEventService;
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public AccessControlContext(
            DbContextOptions<AccessControlContext> options,
            IDomainEventService domainEventService,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _domainEventService = domainEventService;
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
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

#nullable disable

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

#nullable restore

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DispatchEvents();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task DispatchEvents()
        {
            var entities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity);

            var domainEvents = entities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            entities.ToList().ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await _domainEventService.Publish(domainEvent);
        }
    }
}
