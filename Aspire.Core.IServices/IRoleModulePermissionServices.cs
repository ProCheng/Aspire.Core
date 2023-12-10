using Aspire.Core.IServices.BASE;
using Aspire.Core.Model.Models;

namespace Aspire.Core.IServices
{
    /// <summary>
    /// RoleModulePermissionServices
    /// </summary>	
    public interface IRoleModulePermissionServices : IBaseServices<RoleModulePermission>
    {

        Task<List<RoleModulePermission>> GetRoleModule();
        Task<List<TestMuchTableResult>> QueryMuchTable();
        Task<List<RoleModulePermission>> RoleModuleMaps();
        Task<List<RoleModulePermission>> GetRMPMaps();
        /// <summary>
        /// �������²˵���ӿڵĹ�ϵ
        /// </summary>
        /// <param name="permissionId">�˵�����</param>
        /// <param name="moduleId">�ӿ�����</param>
        /// <returns></returns>
        Task UpdateModuleId(long permissionId, long moduleId);
    }
}
