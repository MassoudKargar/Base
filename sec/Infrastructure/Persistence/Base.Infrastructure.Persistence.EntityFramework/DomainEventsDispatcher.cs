namespace Base.Infrastructure.Persistence.EntityFramework;

public class DomainEventsDispatcher(IDomainEventDetector domainEventDetector, IMediator eventBus)
    : IDomainEventsDispatcher
{
    public async Task DispatchEventsAsync()
    {
        var domainEvents = domainEventDetector.GetAndClearDomainEvents();

        var tasks = domainEvents.Select(domainEvent => eventBus.Publish(domainEvent));

        await Task.WhenAll(tasks);
    }
}
