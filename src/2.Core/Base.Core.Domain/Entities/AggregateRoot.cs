namespace Base.Core.Domains.Entities;
/// <summary>
/// Implement the AggregateRoot template
/// You can see the full description of this pattern at the address below
/// https://martinfowler.com/bliki/DDD_Aggregate.html
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{
    /// <summary>
    /// Maintains the list of related events
    /// </summary>
    private readonly List<IDomainEvent> _events;
    protected AggregateRoot() => _events = new();

    /// <summary>
    /// Aggregate builder to create Aggregate from events
    /// </summary>
    /// <param name="events">If the Event already exists, it will be sent to the Aggregate by this parameter</param>
    public AggregateRoot(IEnumerable<IDomainEvent> events)
    {
        if (events == null || !events.Any()) return;
        foreach (var @event in events)
        {
            Mutate(@event);
        }
    }

    protected void Apply(IDomainEvent @event)
    {
        Mutate(@event);
        AddEvent(@event);
    }

    private void Mutate(IDomainEvent @event)
    {
        var onMethod = GetType().GetMethod("On", BindingFlags.Instance | BindingFlags.NonPublic, [@event.GetType()]);
        onMethod?.Invoke(this, new[] { @event });
    }
    /// <summary>
    /// Adds a new event to the set of events in this aggregate.
    /// Aggregates themselves are responsible for creating and sending the event.
    /// </summary>
    /// <param name="event"></param>
    protected void AddEvent(IDomainEvent @event) => _events.Add(@event);

    /// <summary>
    /// Returns a list of events that occurred for the Aggregate as read-only and immutable.
    /// </summary>
    /// <returns>list of events</returns>
    public IEnumerable<IDomainEvent> GetEvents() => _events.AsEnumerable();

    /// <summary>
    /// Clears the events in this Aggregate
    /// </summary>
    public void ClearEvents() => _events.Clear();
}



public abstract class AggregateRoot : AggregateRoot<long>
{

}