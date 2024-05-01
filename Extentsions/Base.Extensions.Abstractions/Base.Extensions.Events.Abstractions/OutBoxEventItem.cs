namespace Base.Extensions.Events.Abstractions;
public abstract class OutBoxEventItem
{
    public virtual long OutBoxEventItemId { get; set; }
    public virtual Guid EventId { get; set; }
    public virtual string AccuredByUserId { get; set; }
    public virtual DateTime AccruedOn { get; set; }
    public virtual string AggregateName { get; set; }
    public virtual string AggregateTypeName { get; set; }
    public virtual string AggregateId { get; set; }
    public virtual string EventName { get; set; }
    public virtual string EventTypeName { get; set; }
    public virtual string EventPayload { get; set; }
    public virtual string? TraceId { get; set; }
    public virtual string? SpanId { get; set; }
    public virtual bool IsProcessed { get; set; }
}