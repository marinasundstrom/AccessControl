using AccessControl.IdentityService.Application.Common.Interfaces;

namespace AccessControl.IdentityService.Infrastructure.Services;

class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
