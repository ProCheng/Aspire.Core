using Aspire.Core.IRepository.Base;
using Aspire.Core.Model.Models;
using Autofac;
using Xunit;

namespace Aspire.Core.Tests
{
    public class Repository_Base_Should
    {
        private IBaseRepository<AspireArticle> baseRepository;
        DI_Test dI_Test = new DI_Test();

        public Repository_Base_Should()
        {

            var container = dI_Test.DICollections();

            baseRepository = container.Resolve<IBaseRepository<AspireArticle>>();

            //DbContext.Init(BaseDBConfig.ConnectionString,(DbType)BaseDBConfig.DbType);
        }


        [Fact]
        public async void Get_Aspires_Test()
        {
            var data = await baseRepository.Query();

            Assert.NotNull(data);
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
                bsubmitter = "xuint： test repositoryBase add Aspire",
            };

            var BId = await baseRepository.Add(AspireArticle);
            Assert.True(BId > 0);
        }


        [Fact]
        public async void Update_Aspire_Test()
        {
            var IsUpd = false;
            var updateModel = (await baseRepository.Query(d => d.btitle == "xuint test title")).FirstOrDefault();

            Assert.NotNull(updateModel);

            updateModel.bcontent = "xuint: test repositoryBase content update";
            updateModel.bCreateTime = DateTime.Now;
            updateModel.bUpdateTime = DateTime.Now;

            IsUpd = await baseRepository.Update(updateModel);

            Assert.True(IsUpd);
        }

        [Fact]
        public async void Delete_Aspire_Test()
        {
            var IsDel = false;
            var deleteModel = (await baseRepository.Query(d => d.btitle == "xuint test title")).FirstOrDefault();

            Assert.NotNull(deleteModel);

            IsDel = await baseRepository.Delete(deleteModel);

            Assert.True(IsDel);
        }
    }
}
