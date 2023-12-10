using Aspire.Core.Common.DB.Extension;
using Aspire.Core.Model.IDS4DbModels;
using Aspire.Core.Services.BASE;

namespace Aspire.Core.IServices
{
    public class ApplicationUserServices : BaseServices<ApplicationUser>, IApplicationUserServices
    {
        public bool IsEnable()
        {
            var configId = typeof(ApplicationUser).GetEntityTenant();
            return Db.AsTenant().IsAnyConnection(configId);
        }
    }
}