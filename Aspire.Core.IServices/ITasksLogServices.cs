using Aspire.Core.IServices.BASE;
using Aspire.Core.Model;
using Aspire.Core.Model.Models;

namespace Aspire.Core.IServices
{
    /// <summary>
    /// ITasksLogServices
    /// </summary>	
    public interface ITasksLogServices : IBaseServices<TasksLog>
    {
        public Task<PageModel<TasksLog>> GetTaskLogs(long jobId, int page, int intPageSize, DateTime? runTime, DateTime? endTime);
        public Task<object> GetTaskOverview(long jobId, DateTime? runTime, DateTime? endTime, string type);
    }
}
