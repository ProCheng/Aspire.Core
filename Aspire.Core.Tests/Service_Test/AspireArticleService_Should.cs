using Aspire.Core.IServices;
using Aspire.Core.Model.Models;
using Autofac;
using Xunit;

namespace Aspire.Core.Tests
{
    public class AspireArticleService_Should
    {

        private IAspireArticleServices AspireArticleServices;
        DI_Test dI_Test = new DI_Test();


        public AspireArticleService_Should()
        {
            //mockAspireRep.Setup(r => r.Query());

            var container = dI_Test.DICollections();

            AspireArticleServices = container.Resolve<IAspireArticleServices>();

        }


        [Fact]
        public void AspireArticleServices_Test()
        {
            Assert.NotNull(AspireArticleServices);
        }


        [Fact]
        public async void Get_Aspires_Test()
        {
            var data = await AspireArticleServices.GetAspires();

            Assert.True(data.Any());
        }

        [Fact]
        public async void Add_Aspire_Test()
        {
            AspireArticle AspireArticle = new AspireArticle()
            {
                bCreateTime = DateTime.Now,
                bUpdateTime = DateTime.Now,
                btitle = "xuint test title",
                bcontent = "xuint test content",
                bsubmitter = "xuint test submitter",
            };

            var BId = await AspireArticleServices.Add(AspireArticle);

            Assert.True(BId > 0);
        }

        [Fact]
        public async void Delete_Aspire_Test()
        {
            Add_Aspire_Test();

            var deleteModel = (await AspireArticleServices.Query(d => d.btitle == "xuint test title")).FirstOrDefault();

            Assert.NotNull(deleteModel);

            var IsDel = await AspireArticleServices.Delete(deleteModel);

            Assert.True(IsDel);
        }
    }
}
