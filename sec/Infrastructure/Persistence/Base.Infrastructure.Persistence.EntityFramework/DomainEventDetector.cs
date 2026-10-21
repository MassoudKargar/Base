namespace Base.Infrastructure.Persistence.EntityFramework;

internal class DomainEventDetector : IDomainEventDetector, IIntegrationEventDetector
{
    private readonly DbContext _dbContext;

    public DomainEventDetector(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<INotification> GetAndClearDomainEvents()
    {
        var domainEntities = GetDomainEntities();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Events)
            .Where(@event => @event is DomainEvent)
            .ToList();

        foreach (var entity in domainEntities)
        {
            entity.Entity.ClearEvents();
        }

        return domainEvents;
    }

    public List<IIntegrationEvent> GetIntegrationEvents()
    {
        var domainEntities = GetDomainEntities();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Events)
            .ToList();

        return domainEvents
             .Where(n => n is IIntegrationEvent)
             .Cast<IIntegrationEvent>()
             .ToList();
    }

    private List<EntityEntry<IAggregateRoot>> GetDomainEntities()
    {
        return _dbContext.ChangeTracker
        .Entries<IAggregateRoot>()
        .Where(x => x.Entity.Events != null && x.Entity.Events.Any()).ToList();
    }
}
