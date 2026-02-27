using Microsoft.Extensions.Logging;

namespace Base.Extensions.MessageBus.Abstractions.Fakes;

public sealed class FakeReceiveMessageBus : IReceiveMessageBus
{
    private readonly ILogger<FakeReceiveMessageBus> _logger;

    public FakeReceiveMessageBus(ILogger<FakeReceiveMessageBus> logger)
    {
        _logger = logger;
    }

    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fake message bus initialized.");
        return Task.CompletedTask;
    }

    public Task SubscribeAsync(
        string serviceId,
        string eventName,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Fake subscribe for event {eventName} from service {serviceId}",
            eventName,
            serviceId);

        return Task.CompletedTask;
    }

    public Task ReceiveAsync(
        string commandName,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Fake receive command {commandName}",
            commandName);

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        _logger.LogInformation("Fake message bus disposed.");
        return ValueTask.CompletedTask;
    }
}