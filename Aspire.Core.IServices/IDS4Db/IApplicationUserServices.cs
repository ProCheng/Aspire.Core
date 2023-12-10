using Aspire.Core.IServices.BASE;
using Aspire.Core.Model.IDS4DbModels;

namespace Aspire.Core.IServices
{
    public partial interface IApplicationUserServices : IBaseServices<ApplicationUser>
    {
        bool IsEnable();
    }
}