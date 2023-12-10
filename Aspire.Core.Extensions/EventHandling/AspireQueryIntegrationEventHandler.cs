using Aspire.Core.Common;
using Aspire.Core.EventBus.EventHandling;
using Aspire.Core.IServices;
using Microsoft.Extensions.Logging;

namespace Aspire.Core.EventBus
{
    public class AspireQueryIntegrationEventHandler : IIntegrationEventHandler<AspireQueryIntegrationEvent>
    {
        private readonly IAspireArticleServices _AspireArticleServices;
        private readonly ILogger<AspireQueryIntegrationEventHandler> _logger;

        public AspireQueryIntegrationEventHandler(
            IAspireArticleServices AspireArticleServices,
            ILogger<AspireQueryIntegrationEventHandler> logger)
        {
            _AspireArticleServices = AspireArticleServices;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AspireQueryIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "Aspire.Core", @event);

            ConsoleHelper.WriteSuccessLine($"----- Handling integration event: {@event.Id} at Aspire.Core - ({@event})");

            await _AspireArticleServices.QueryById(@event.AspireId.ToString());
        }

    }
}
