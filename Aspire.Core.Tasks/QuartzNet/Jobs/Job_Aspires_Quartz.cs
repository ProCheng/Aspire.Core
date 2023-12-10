
using Aspire.Core.IServices;
using Quartz;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到
/// </summary>
namespace Aspire.Core.Tasks
{
    public class Job_Aspires_Quartz : JobBase, IJob
    {
        private readonly IAspireArticleServices _AspireArticleServices;

        public Job_Aspires_Quartz(IAspireArticleServices AspireArticleServices, ITasksQzServices tasksQzServices, ITasksLogServices tasksLogServices)
            : base(tasksQzServices, tasksLogServices)
        {
            _AspireArticleServices = AspireArticleServices;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var executeLog = await ExecuteJob(context, async () => await Run(context));
        }
        public async Task Run(IJobExecutionContext context)
        {
            System.Console.WriteLine($"Job_Aspires_Quartz 执行 {DateTime.Now.ToShortTimeString()}");
            var list = await _AspireArticleServices.Query();
            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;
            //int jobId = data.GetInt("JobParam");
        }
    }
}
