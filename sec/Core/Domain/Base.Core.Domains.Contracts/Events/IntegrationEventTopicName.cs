namespace Base.Core.Domains.Contracts.Events;
internal class IntegrationEventTopicName : IIntegrationEventTopicName
{
    public string Calculate(IIntegrationEvent integrationEvent)
    {
        return integrationEvent.GetType().FullName
            ?? throw new NullReferenceException("FullName");
    }
}
