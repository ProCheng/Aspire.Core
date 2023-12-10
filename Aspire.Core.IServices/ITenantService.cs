using Aspire.Core.IServices.BASE;
using Aspire.Core.Model.Models;

namespace Aspire.Core.IServices;

public interface ITenantService : IBaseServices<SysTenant>
{
    public Task SaveTenant(SysTenant tenant);

    public Task InitTenantDb(SysTenant tenant);
}