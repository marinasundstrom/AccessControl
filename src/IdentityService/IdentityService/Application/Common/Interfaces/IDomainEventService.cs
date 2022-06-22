using AccessControl.IdentityService.Domain.Common;

namespace AccessControl.IdentityService.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
