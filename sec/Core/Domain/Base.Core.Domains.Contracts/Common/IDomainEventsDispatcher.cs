namespace Base.Core.Domains.Contracts.Common;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}
