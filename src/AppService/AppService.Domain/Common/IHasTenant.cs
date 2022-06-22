
namespace AppService.Domain.Common;

public interface IHasTenant
{
    public string TenantId { get; set; }
}