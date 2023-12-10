using Aspire.Core.IServices.BASE;
using Aspire.Core.Model;
using Aspire.Core.Model.Models;

namespace Aspire.Core.IServices
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        Task<MessageModel<string>> TestTranInRepository();
        Task<bool> TestTranInRepositoryAOP();

        Task<bool> TestTranPropagation();

        Task<bool> TestTranPropagationNoTran();

        Task<bool> TestTranPropagationTran();
    }
}