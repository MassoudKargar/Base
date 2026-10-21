namespace Base.Core.Domains.Contracts.Events;

public interface IIntegrationEventTopicName
{
    public string Calculate(IIntegrationEvent integrationEvent);
}