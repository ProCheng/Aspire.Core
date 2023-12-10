using Aspire.Core.IServices;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aspire.Core.Filter
{
    public class UseServiceDIAttribute : ActionFilterAttribute
    {

        protected readonly ILogger<UseServiceDIAttribute> _logger;
        private readonly IAspireArticleServices _AspireArticleServices;
        private readonly string _name;

        public UseServiceDIAttribute(ILogger<UseServiceDIAttribute> logger, IAspireArticleServices AspireArticleServices, string Name = "")
        {
            _logger = logger;
            _AspireArticleServices = AspireArticleServices;
            _name = Name;
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var dd = _AspireArticleServices.Query().Result;
            _logger.LogInformation("测试自定义服务特性");
            Console.WriteLine(_name);
            base.OnActionExecuted(context);
            DeleteSubscriptionFiles();
        }

        private void DeleteSubscriptionFiles()
        {

        }
    }
}
