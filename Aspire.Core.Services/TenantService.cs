using Aspire.Core.Common.DB;
using Aspire.Core.Common.Seed;
using Aspire.Core.IServices;
using Aspire.Core.Model.Models;
using Aspire.Core.Repository.UnitOfWorks;
using Aspire.Core.Services.BASE;

namespace Aspire.Core.Services;

public class TenantService : BaseServices<SysTenant>, ITenantService
{
    private readonly IUnitOfWorkManage _uowManager;

    public TenantService(IUnitOfWorkManage uowManage)
    {
        this._uowManager = uowManage;
    }


    public async Task SaveTenant(SysTenant tenant)
    {
        bool initDb = tenant.Id == 0;
        using (var uow = _uowManager.CreateUnitOfWork())
        {

            tenant.DefaultTenantConfig();

            if (tenant.Id == 0)
            {
                await Db.Insertable(tenant).ExecuteReturnSnowflakeIdAsync();
            }
            else
            {
                var oldTenant = await QueryById(tenant.Id);
                if (oldTenant.Connection != tenant.Connection)
                {
                    initDb = true;
                }

                await Db.Updateable(tenant).ExecuteCommandAsync();
            }

            uow.Commit();
        }

        if (initDb)
        {
            await InitTenantDb(tenant);
        }
    }

    public async Task InitTenantDb(SysTenant tenant)
    {
        await DBSeed.InitTenantSeedAsync(Db.AsTenant(), tenant.GetConnectionConfig());
    }
}