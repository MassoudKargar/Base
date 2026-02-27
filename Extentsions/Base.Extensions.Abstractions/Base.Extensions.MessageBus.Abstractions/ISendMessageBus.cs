namespace Base.Extensions.MessageBus.Abstractions;

/// <summary>
/// مسئول ارسال پیام‌های دستوری و رویدادی به زیرساخت پیام‌رسان
/// </summary>
public interface ISendMessageBus
{
    Task PublishAsync<TEvent>(
        TEvent @event,
        MessageContext? context = null,
        CancellationToken cancellationToken = default)
        where TEvent : class;

    Task SendCommandAsync<TCommand>(
        string destinationService,
        TCommand command,
        MessageContext? context = null,
        CancellationToken cancellationToken = default)
        where TCommand : class;

    Task SendAsync(
        Parcel parcel,
        CancellationToken cancellationToken = default);
}