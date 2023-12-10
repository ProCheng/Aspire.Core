using Aspire.Core.IServices.BASE;
using Aspire.Core.Model.Models;
using Aspire.Core.Model.ViewModels;

namespace Aspire.Core.IServices
{
    public interface IAspireArticleServices : IBaseServices<AspireArticle>
    {
        Task<List<AspireArticle>> GetAspires();
        Task<AspireViewModels> GetAspireDetails(long id);

    }

}
