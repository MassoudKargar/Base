namespace Base.Core.Domains.Contracts.Common;

public interface IAggregateRoot
{
    IList<INotification> Events { get; }

    void ClearEvents();

    void MarkAsDelete();
    bool IsMarkAsDeleted();
    void RaiseEvent(INotification @event);
}