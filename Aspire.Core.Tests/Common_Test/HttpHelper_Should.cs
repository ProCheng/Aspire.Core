using Aspire.Core.Common.Helper;
using Xunit;

namespace Aspire.Core.Tests.Common_Test
{
    public class HttpHelper_Should
    {

        [Fact]
        public void Get_Async_Test()
        {
            var responseString = HttpHelper.GetAsync("").Result;

            Assert.NotNull(responseString);
        }

        [Fact]
        public void Post_Async_Test()
        {
            var responseString = HttpHelper.PostAsync("", "{\"name\":\"admin\",\"pwd\":\"admin\"}").Result;

            Assert.NotNull(responseString);
        }

    }
}
