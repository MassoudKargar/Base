namespace Base.Core.Domains.Contracts.Events;

public interface IScheduleIntegrationEventBus
{
    public Task PublishAsync<TEvent>(TEvent eventObject, DateTimeOffset publishDate)
        where TEvent : IIntegrationEvent;
}