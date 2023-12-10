using Aspire.Core.IServices;
using InitQ.Abstractions;
using InitQ.Attributes;

namespace Aspire.Core.Extensions.Redis
{
    public class RedisSubscribe : IRedisSubscribe
    {
        private readonly IAspireArticleServices _AspireArticleServices;

        public RedisSubscribe(IAspireArticleServices AspireArticleServices)
        {
            _AspireArticleServices = AspireArticleServices;
        }

        [Subscribe(RedisMqKey.Loging)]
        private async Task SubRedisLoging(string msg)
        {
            Console.WriteLine($"订阅者 1 从 队列{RedisMqKey.Loging} 消费到/接受到 消息:{msg}");

            await Task.CompletedTask;
        }
    }
}
