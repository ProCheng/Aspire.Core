

using Aspire.Core.IServices.BASE;
using Aspire.Core.Model.Models;

namespace Aspire.Core.IServices
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>	
    public interface ISysUserInfoServices : IBaseServices<SysUserInfo>
    {
        Task<SysUserInfo> SaveUserInfo(string loginName, string loginPwd);
        Task<string> GetUserRoleNameStr(string loginName, string loginPwd);
    }
}
