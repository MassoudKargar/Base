namespace Base.Core.Domains.Entities;

public interface IAggregateRoot
{
    void ClearEvents();
    IEnumerable<IDomainEvent> GetEvents();
}