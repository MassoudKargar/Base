namespace Base.Extensions.MessageBus.Abstractions;

public interface IReceiveMessageBus : IAsyncDisposable
{
    Task InitializeAsync(CancellationToken cancellationToken = default);

    Task SubscribeAsync(string serviceId, string eventName, CancellationToken cancellationToken = default);

    Task ReceiveAsync(string commandName, CancellationToken cancellationToken = default);
}