namespace Base.Core.Domains.Contracts.Events;

public abstract class DomainEvent : INotification
{
    protected DomainEvent()
    {
        OccurredOn = DateTimeOffset.Now;
    }

    protected DomainEvent(string aggregateId)
    {
        AggregateId = aggregateId;
        OccurredOn = DateTimeOffset.Now;
    }

    public DateTimeOffset OccurredOn { get; set; }

    public string AggregateId { get; private set; }

    public long AggregateVersion { get; private set; }

    public DomainEventUserInfo UserInfo { get; set; }

    public void SetUserContextValue(DomainEventUserInfo userInfo)
    {
        UserInfo = userInfo;
    }

    public void SetAggregateVersion(long aggregateVersion)
    {
        AggregateVersion = aggregateVersion;
    }

    public void SetAggregateId(string aggregateId)
    {
        AggregateId = aggregateId;
    }
}