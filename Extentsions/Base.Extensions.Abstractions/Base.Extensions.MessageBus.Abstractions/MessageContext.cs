namespace Base.Extensions.MessageBus.Abstractions;

public sealed class MessageContext
{
    public string? CorrelationId { get; init; }
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public IDictionary<string, object>? Headers { get; init; }
}