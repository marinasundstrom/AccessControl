using AppService.Domain.Entities;

namespace AppService.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }
    public string CreatedById { get; set; } = null!;
    public User CreatedBy { get; set; } = null!;

    public DateTime? LastModified { get; set; }
    public string? LastModifiedById { get; set; }
    public User? LastModifiedBy { get; set; }
}