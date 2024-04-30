using Base.Core.Domain.Events;

namespace Base.Core.Domain.Entities;

public interface IAggregateRoot
{
    void ClearEvents();
    IEnumerable<IDomainEvent> GetEvents();
}