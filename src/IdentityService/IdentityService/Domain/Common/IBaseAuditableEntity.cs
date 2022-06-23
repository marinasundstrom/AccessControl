using AccessControl.IdentityService.Domain.Entities;

namespace AccessControl.IdentityService.Domain.Common
{
    public interface IAuditableEntity
    {
        DateTime Created { get; set; }
        string CreatedById { get; set; }
        User CreatedBy { get; set; }
        DateTime? LastModified { get; set; }
        string LastModifiedById { get; set; }
        User LastModifiedBy { get; set; }
    }
}