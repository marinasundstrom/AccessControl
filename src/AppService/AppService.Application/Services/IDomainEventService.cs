using AppService.Domain.Common;
using AppService.Domain;

namespace AppService.Application.Services;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}