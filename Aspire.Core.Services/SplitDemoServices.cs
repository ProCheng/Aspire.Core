using Aspire.Core.IRepository.Base;
using Aspire.Core.IServices;
using Aspire.Core.Model.Models;
using Aspire.Core.Services.BASE;

namespace Aspire.Core.FrameWork.Services
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>	
    public class SplitDemoServices : BaseServices<SplitDemo>, ISplitDemoServices
    {
        private readonly IBaseRepository<SplitDemo> _splitDemoRepository;
        public SplitDemoServices(IBaseRepository<SplitDemo> splitDemoRepository)
        {
            _splitDemoRepository = splitDemoRepository;
        }


    }
}
