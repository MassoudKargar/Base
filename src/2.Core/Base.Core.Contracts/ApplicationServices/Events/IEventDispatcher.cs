using Base.Core.Domain.Events;

namespace Base.Core.Contracts.ApplicationServices.Events;

public interface IEventDispatcher
{
    Task PublishDomainEventAsync<TDomainEvent>(TDomainEvent @event) where TDomainEvent : class, IDomainEvent;
}