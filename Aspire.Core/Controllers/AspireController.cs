using Aspire.Core.Common.Helper;
using Aspire.Core.IServices;
using Aspire.Core.Model;
using Aspire.Core.Model.Models;
using Aspire.Core.Model.ViewModels;
using Aspire.Core.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StackExchange.Profiling;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using static Aspire.Core.Extensions.CustomApiVersion;

namespace Aspire.Core.Controllers
{
    /// <summary>
    /// 博客管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/Aspire")]
    public class AspireController : BaseApiController
    {
        public IAspireArticleServices _AspireArticleServices { get; set; }
        private readonly ILogger<AspireController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// 
        public AspireController(ILogger<AspireController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// 获取博客列表【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="bcategory"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<AspireArticle>>> Get(int id, int page = 1, string bcategory = "技术博文", string key = "")
        {
            int intPageSize = 6;
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            Expression<Func<AspireArticle, bool>> whereExpression = a => (a.bcategory == bcategory && a.IsDeleted == false) && ((a.btitle != null && a.btitle.Contains(key)) || (a.bcontent != null && a.bcontent.Contains(key)));

            var pageModelAspire = await _AspireArticleServices.QueryPage(whereExpression, page, intPageSize, " bID desc ");

            using (MiniProfiler.Current.Step("获取成功后，开始处理最终数据"))
            {
                foreach (var item in pageModelAspire.data)
                {
                    if (!string.IsNullOrEmpty(item.bcontent))
                    {
                        item.bRemark = (HtmlHelper.ReplaceHtmlTag(item.bcontent)).Length >= 200 ? (HtmlHelper.ReplaceHtmlTag(item.bcontent)).Substring(0, 200) : (HtmlHelper.ReplaceHtmlTag(item.bcontent));
                        int totalLength = 500;
                        if (item.bcontent.Length > totalLength)
                        {
                            item.bcontent = item.bcontent.Substring(0, totalLength);
                        }
                    }
                }
            }

            return SuccessPage(pageModelAspire);
        }


        /// <summary>
        /// 获取博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[Authorize(Policy = "Scope_AspireModule_Policy")]
        [Authorize]
        public async Task<MessageModel<AspireViewModels>> Get(long id)
        {
            return Success(await _AspireArticleServices.GetAspireDetails(id));
        }


        /// <summary>
        /// 获取详情【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DetailNuxtNoPer")]
        public async Task<MessageModel<AspireViewModels>> DetailNuxtNoPer(long id)
        {
            _logger.LogInformation("xxxxxxxxxxxxxxxxxxx");
            Log.Information("yyyyyyyyyyyyyyyyy");
            return Success(await _AspireArticleServices.GetAspireDetails(id));
        }

        [HttpGet]
        [Route("GoUrl")]
        public async Task<IActionResult> GoUrl(long id = 0)
        {
            var response = await _AspireArticleServices.QueryById(id);
            if (response != null && response.bsubmitter.IsNotEmptyOrNull())
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                if (Regex.IsMatch(response.bsubmitter, Url))
                {
                    response.btraffic += 1;
                    await _AspireArticleServices.Update(response);
                    return Redirect(response.bsubmitter);
                }

            }

            return Ok();
        }

        [HttpGet]
        [Route("GetAspiresByTypesForMVP")]
        public async Task<MessageModel<List<AspireArticle>>> GetAspiresByTypesForMVP(string types = "", int id = 0)
        {
            if (types.IsNotEmptyOrNull())
            {
                var Aspires = await _AspireArticleServices.Query(d => d.bcategory != null && types.Contains(d.bcategory) && d.IsDeleted == false, d => d.bID, false);
                return Success(Aspires);
            }
            return Success(new List<AspireArticle>() { });
        }

        [HttpGet]
        [Route("GetAspireByIdForMVP")]
        public async Task<MessageModel<AspireArticle>> GetAspireByIdForMVP(long id = 0)
        {
            if (id > 0)
            {
                return Success(await _AspireArticleServices.QueryById(id));
            }
            return Success(new AspireArticle());
        }

        /// <summary>
        /// 获取博客测试信息 v2版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        ////MVC自带特性 对 api 进行组管理
        //[ApiExplorerSettings(GroupName = "v2")]
        ////路径 如果以 / 开头，表示绝对路径，反之相对 controller 的想u地路径
        //[Route("/api/v2/Aspire/Aspiretest")]
        //和上边的版本控制以及路由地址都是一样的

        [CustomRoute(ApiVersions.v2, "Aspiretest")]
        public MessageModel<string> V2_Aspiretest()
        {
            return Success<string>("我是第二版的博客信息");
        }

        /// <summary>
        /// 添加博客【无权限】
        /// </summary>
        /// <param name="AspireArticle"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Policy = "Scope_AspireModule_Policy")]
        [Authorize]
        public async Task<MessageModel<string>> Post([FromBody] AspireArticle AspireArticle)
        {
            if (AspireArticle.btitle.Length > 5 && AspireArticle.bcontent.Length > 50)
            {

                AspireArticle.bCreateTime = DateTime.Now;
                AspireArticle.bUpdateTime = DateTime.Now;
                AspireArticle.IsDeleted = false;
                AspireArticle.bcategory = "技术博文";
                var id = (await _AspireArticleServices.Add(AspireArticle));
                return id > 0 ? Success<string>(id.ObjToString()) : Failed("添加失败");
            }
            else
            {
                return Failed("文章标题不能少于5个字符，内容不能少于50个字符！");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="AspireArticle"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddForMVP")]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> AddForMVP([FromBody] AspireArticle AspireArticle)
        {
            AspireArticle.bCreateTime = DateTime.Now;
            AspireArticle.bUpdateTime = DateTime.Now;
            AspireArticle.IsDeleted = false;
            var id = (await _AspireArticleServices.Add(AspireArticle));
            return id > 0 ? Success<string>(id.ObjToString()) : Failed("添加失败");
        }
        /// <summary>
        /// 更新博客信息
        /// </summary>
        /// <param name="AspireArticle"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        [Route("Update")]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> Put([FromBody] AspireArticle AspireArticle)
        {
            if (AspireArticle != null && AspireArticle.bID > 0)
            {
                var model = await _AspireArticleServices.QueryById(AspireArticle.bID);

                if (model != null)
                {
                    model.btitle = AspireArticle.btitle;
                    model.bcategory = AspireArticle.bcategory;
                    model.bsubmitter = AspireArticle.bsubmitter;
                    model.bcontent = AspireArticle.bcontent;
                    model.btraffic = AspireArticle.btraffic;

                    if (await _AspireArticleServices.Update(model))
                    {
                        return Success<string>(AspireArticle?.bID.ObjToString());
                    }
                }
            }
            return Failed("更新失败");
        }



        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Permissions.Name)]
        [Route("Delete")]
        public async Task<MessageModel<string>> Delete(long id)
        {
            if (id > 0)
            {
                var AspireArticle = await _AspireArticleServices.QueryById(id);
                if (AspireArticle == null)
                {
                    return Failed("查询无数据");
                }
                AspireArticle.IsDeleted = true;
                return await _AspireArticleServices.Update(AspireArticle) ? Success(AspireArticle?.bID.ObjToString(), "删除成功") : Failed("删除失败");
            }
            return Failed("入参无效");
        }
        /// <summary>
        /// apache jemeter 压力测试
        /// 更新接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ApacheTestUpdate")]
        public async Task<MessageModel<bool>> ApacheTestUpdate()
        {
            return Success(await _AspireArticleServices.Update(new { bsubmitter = $"laozhang{DateTime.Now.Millisecond}", bID = 1 }), "更新成功");
        }
    }
}