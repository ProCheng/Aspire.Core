namespace Aspire.Core.EventBus.EventHandling
{
    public class AspireQueryIntegrationEvent : IntegrationEvent
    {
        public string AspireId { get; private set; }

        public AspireQueryIntegrationEvent(string Aspireid)
            => AspireId = Aspireid;
    }
}
