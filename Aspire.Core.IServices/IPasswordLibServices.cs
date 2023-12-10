using Aspire.Core.IServices.BASE;
using Aspire.Core.Model.Models;

namespace Aspire.Core.IServices
{
    public partial interface IPasswordLibServices : IBaseServices<PasswordLib>
    {
        Task<bool> TestTranPropagation2();
        Task<bool> TestTranPropagationNoTranError();
        Task<bool> TestTranPropagationTran2();
    }
}
