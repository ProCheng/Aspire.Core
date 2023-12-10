using Xunit;

namespace Aspire.Core.Tests
{
    public class Redis_Should
    {
        DI_Test dI_Test = new DI_Test();

        public Redis_Should()
        {
            //var container = dI_Test.DICollections();
            //_redisCacheManager = container.Resolve<IRedisCacheManager>();

        }

        [Fact]
        public void Connect_Redis_Test()
        {

            //var redisAspireCache = _redisCacheManager.Get<object>("Redis.Aspire");

            //Assert.Null(redisAspireCache);
        }

    }
}
