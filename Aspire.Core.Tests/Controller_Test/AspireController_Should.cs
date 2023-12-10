using Aspire.Core.Controllers;
using Aspire.Core.IServices;
using Aspire.Core.Model;
using Aspire.Core.Model.Models;
using Aspire.Core.Model.ViewModels;
using Autofac;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Aspire.Core.Tests
{
    public class AspireController_Should
    {
        Mock<IAspireArticleServices> mockAspireSev = new Mock<IAspireArticleServices>();
        Mock<ILogger<AspireController>> mockLogger = new Mock<ILogger<AspireController>>();
        AspireController AspireController;

        private IAspireArticleServices AspireArticleServices;
        DI_Test dI_Test = new DI_Test();



        public AspireController_Should()
        {
            mockAspireSev.Setup(r => r.Query());

            var container = dI_Test.DICollections();
            AspireArticleServices = container.Resolve<IAspireArticleServices>();
            AspireController = new AspireController(mockLogger.Object);
            AspireController._AspireArticleServices = AspireArticleServices;
        }

        [Fact]
        public void TestEntity()
        {
            AspireArticle AspireArticle = new AspireArticle();

            Assert.True(AspireArticle.bID >= 0);
        }

        [Fact]
        public async void Get_Aspire_Page_Test()
        {
            MessageModel<PageModel<AspireArticle>> Aspires = await AspireController.Get(1, 1, "技术博文", "");
            Assert.NotNull(Aspires);
            Assert.NotNull(Aspires.response);
            Assert.True(Aspires.response.dataCount >= 0);
        }

        [Fact]
        public async void Get_Aspire_Test()
        {
            MessageModel<AspireViewModels> AspireVo = await AspireController.Get(1.ObjToLong());

            Assert.NotNull(AspireVo);
        }

        [Fact]
        public async void Get_Aspire_For_Nuxt_Test()
        {
            MessageModel<AspireViewModels> AspireVo = await AspireController.DetailNuxtNoPer(1);

            Assert.NotNull(AspireVo);
        }

        [Fact]
        public async void Get_Go_Url_Test()
        {
            object urlAction = await AspireController.GoUrl(1);

            Assert.NotNull(urlAction);
        }

        [Fact]
        public async void Get_Aspire_By_Type_For_MVP_Test()
        {
            MessageModel<List<AspireArticle>> Aspires = await AspireController.GetAspiresByTypesForMVP("技术博文");

            Assert.NotNull(Aspires);
            Assert.True(Aspires.success);
            Assert.NotNull(Aspires.response);
            Assert.True(Aspires.response.Count >= 0);
        }

        [Fact]
        public async void Get_Aspire_By_Id_For_MVP_Test()
        {
            MessageModel<AspireArticle> Aspire = await AspireController.GetAspireByIdForMVP(1);

            Assert.NotNull(Aspire);
            Assert.True(Aspire.success);
            Assert.NotNull(Aspire.response);
        }

        [Fact]
        public async void PostTest()
        {
            AspireArticle AspireArticle = new AspireArticle()
            {
                bCreateTime = DateTime.Now,
                bUpdateTime = DateTime.Now,
                btitle = "xuint :test controller addEntity",
                bcontent = "xuint :test controller addEntity. this is content.this is content."
            };

            var res = await AspireController.Post(AspireArticle);

            Assert.True(res.success);

            var data = res.response;

            Assert.NotNull(data);
        }

        [Fact]
        public async void Post_Insert_For_MVP_Test()
        {
            AspireArticle AspireArticle = new AspireArticle()
            {
                bCreateTime = DateTime.Now,
                bUpdateTime = DateTime.Now,
                btitle = "xuint :test controller addEntity",
                bcontent = "xuint :test controller addEntity. this is content.this is content."
            };

            var res = await AspireController.AddForMVP(AspireArticle);

            Assert.True(res.success);

            var data = res.response;

            Assert.NotNull(data);
        }

        [Fact]
        public async void Put_Test()
        {
            AspireArticle AspireArticle = new AspireArticle()
            {
                bID = 1,
                bCreateTime = DateTime.Now,
                bUpdateTime = DateTime.Now,
                btitle = "xuint put :test controller addEntity",
                bcontent = "xuint put :test controller addEntity. this is content.this is content."
            };

            var res = await AspireController.Put(AspireArticle);

            Assert.True(res.success);

            var data = res.response;

            Assert.NotNull(data);
        }

        [Fact]
        public async void Delete_Test()
        {
            var res = await AspireController.Delete(99);

            Assert.False(res.success);

            var data = res.response;

            Assert.Null(data);
        }

        [Fact]
        public async void Apache_Update_Test()
        {
            var res = await AspireController.ApacheTestUpdate();

            Assert.True(res.success);
        }
    }
}
