using Aspire.Core.IServices.BASE;
using Aspire.Core.Model.Models;

namespace Aspire.Core.IServices
{
    /// <summary>
    /// RoleServices
    /// </summary>	
    public interface IRoleServices : IBaseServices<Role>
    {
        Task<Role> SaveRole(string roleName);
        Task<string> GetRoleNameByRid(int rid);

    }
}
