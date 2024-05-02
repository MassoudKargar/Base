namespace Base.Core.Contracts.ApplicationServices.Events;

public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent @event);
}