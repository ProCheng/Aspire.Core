using Aspire.Core.IServices;
using Aspire.Core.Model.Models;
using Aspire.Core.Repository.UnitOfWorks;
using Aspire.Core.Services.BASE;
using Microsoft.Extensions.Logging;

namespace Aspire.Core.Services
{
    /// <summary>
	/// WeChatCompanyServices
	/// </summary>
    public class WeChatCompanyServices : BaseServices<WeChatCompany>, IWeChatCompanyServices
    {
        readonly IUnitOfWorkManage _unitOfWorkManage;
        readonly ILogger<WeChatCompanyServices> _logger;
        public WeChatCompanyServices(IUnitOfWorkManage unitOfWorkManage, ILogger<WeChatCompanyServices> logger)
        {
            this._unitOfWorkManage = unitOfWorkManage;
            this._logger = logger;
        }

    }
}