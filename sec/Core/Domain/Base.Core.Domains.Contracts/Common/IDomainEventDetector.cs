namespace Base.Core.Domains.Contracts.Common;

public interface IDomainEventDetector
{
    IEnumerable<INotification> GetAndClearDomainEvents();
}
