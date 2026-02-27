namespace Base.Extensions.MessageBus.Abstractions;

/// <summary>
/// Envelope پیام جهت انتقال از طریق زیرساخت پیام‌رسان
/// </summary>
public sealed class Parcel
{
    public string MessageId { get; }
    public string? CorrelationId { get; }
    public string MessageName { get; }
    public IReadOnlyDictionary<string, string> Headers { get; }
    public string Body { get; }
    public string Route { get; } 
    public Parcel(
        string messageName,
        string? body, string route, 
        string? correlationId = null,
        string? messageId = null,
        IReadOnlyDictionary<string, string>? headers = null)
    {
        MessageName = messageName ?? throw new ArgumentNullException(nameof(messageName));
        Route = route;
        Body = body??string.Empty;
        CorrelationId = correlationId;
        MessageId = messageId ?? Guid.NewGuid().ToString("N");
        Headers = headers ?? new Dictionary<string, string>();
    }
}