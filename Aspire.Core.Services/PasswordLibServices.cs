using Aspire.Core.Common;
using Aspire.Core.Common.DB;
using Aspire.Core.IRepository.Base;
using Aspire.Core.IServices;
using Aspire.Core.Model.Models;
using Aspire.Core.Services.BASE;

namespace Aspire.Core.Services
{
    public partial class PasswordLibServices : BaseServices<PasswordLib>, IPasswordLibServices
    {
        IBaseRepository<PasswordLib> _dal;

        public PasswordLibServices(IBaseRepository<PasswordLib> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        [UseTran(Propagation = Propagation.Required)]
        public async Task<bool> TestTranPropagation2()
        {
            await _dal.Add(new PasswordLib()
            {
                IsDeleted = false,
                plAccountName = "aaa",
                plCreateTime = DateTime.Now
            });

            return true;
        }

        [UseTran(Propagation = Propagation.Mandatory)]
        public async Task<bool> TestTranPropagationNoTranError()
        {
            await _dal.Add(new PasswordLib()
            {
                IsDeleted = false,
                plAccountName = "aaa",
                plCreateTime = DateTime.Now
            });

            return true;
        }

        [UseTran(Propagation = Propagation.Nested)]
        public async Task<bool> TestTranPropagationTran2()
        {
            await _dal.Add(new PasswordLib()
            {
                IsDeleted = false,
                plAccountName = "aaa",
                plCreateTime = DateTime.Now
            });

            return true;
        }
    }
}