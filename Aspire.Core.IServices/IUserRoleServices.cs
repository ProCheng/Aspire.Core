using Aspire.Core.IServices.BASE;
using Aspire.Core.Model.Models;

namespace Aspire.Core.IServices
{
    /// <summary>
    /// UserRoleServices
    /// </summary>	
    public interface IUserRoleServices : IBaseServices<UserRole>
    {

        Task<UserRole> SaveUserRole(long uid, long rid);
        Task<int> GetRoleIdByUid(long uid);
    }
}

