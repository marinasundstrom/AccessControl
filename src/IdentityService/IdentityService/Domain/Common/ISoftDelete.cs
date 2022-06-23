
using AccessControl.IdentityService.Domain.Entities;

namespace AccessControl.IdentityService.Domain.Common;

public interface ISoftDelete
{
    DateTime? Deleted { get; set; }

    string? DeletedById { get; set; }

    User? DeletedBy { get; set; }
}
