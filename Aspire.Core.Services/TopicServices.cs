using Aspire.Core.Common;
using Aspire.Core.IServices;
using Aspire.Core.Model.Models;
using Aspire.Core.Services.BASE;

namespace Aspire.Core.Services
{
    public class TopicServices : BaseServices<Topic>, ITopicServices
    {
        /// <summary>
        /// 获取开Bug专题分类（缓存）
        /// </summary>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 60)]
        public async Task<List<Topic>> GetTopics()
        {
            return await base.Query(a => !a.tIsDelete && a.tSectendDetail == "tbug");
        }

    }
}
