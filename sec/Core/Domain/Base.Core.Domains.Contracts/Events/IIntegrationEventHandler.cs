namespace Base.Core.Domains.Contracts.Events;

public interface IIntegrationEventHandler<in TEvent> where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent eventToHandle);
}
