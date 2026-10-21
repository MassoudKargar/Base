namespace Base.Core.Domains.Contracts.Events;

public interface IRecurringIntegrationEventBus
{
    public Task PublishAsync<TEvent>(string recurringJobId, TEvent eventObject, string cronExpression)
        where TEvent : IIntegrationEvent;

    public Task RemoveAsync(string recurringJobId);

}