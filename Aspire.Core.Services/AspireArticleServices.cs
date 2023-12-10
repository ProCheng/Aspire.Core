using Aspire.Core.Common;
using Aspire.Core.IServices;
using Aspire.Core.Model.Models;
using Aspire.Core.Model.ViewModels;
using Aspire.Core.Services.BASE;
using AutoMapper;

namespace Aspire.Core.Services
{
    public class AspireArticleServices : BaseServices<AspireArticle>, IAspireArticleServices
    {
        IMapper _mapper;
        public AspireArticleServices(IMapper mapper)
        {
            this._mapper = mapper;
        }
        /// <summary>
        /// 获取视图博客详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AspireViewModels> GetAspireDetails(long id)
        {
            // 此处想获取上一条下一条数据，因此将全部数据list出来，有好的想法请提出
            //var Aspirelist = await base.Query(a => a.IsDeleted==false, a => a.bID);
            var AspireArticle = (await base.Query(a => a.bID == id && a.bcategory == "技术博文")).FirstOrDefault();

            AspireViewModels models = null;

            if (AspireArticle != null)
            {
                models = _mapper.Map<AspireViewModels>(AspireArticle);

                //要取下一篇和上一篇，以当前id开始，按id排序后top(2)，而不用取出所有记录
                //这样在记录很多的时候也不会有多大影响
                var nextAspires = await base.Query(a => a.bID >= id && a.IsDeleted == false && a.bcategory == "技术博文", 2, "bID");
                if (nextAspires.Count == 2)
                {
                    models.next = nextAspires[1].btitle;
                    models.nextID = nextAspires[1].bID;
                }
                var prevAspires = await base.Query(a => a.bID <= id && a.IsDeleted == false && a.bcategory == "技术博文", 2, "bID desc");
                if (prevAspires.Count == 2)
                {
                    models.previous = prevAspires[1].btitle;
                    models.previousID = prevAspires[1].bID;
                }

                AspireArticle.btraffic += 1;
                await base.Update(AspireArticle, new List<string> { "btraffic" });
            }

            return models;

        }


        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 10)]
        public async Task<List<AspireArticle>> GetAspires()
        {
            var Aspirelist = await base.Query(a => a.bID > 0, a => a.bID);

            return Aspirelist;

        }
    }
}
