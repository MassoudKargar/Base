using Microsoft.Extensions.Logging;

namespace Base.Extensions.MessageBus.Abstractions.Fakes;

public sealed class FakeSendMessageBus : ISendMessageBus
{
    private readonly ILogger<FakeSendMessageBus> _logger;

    public List<object> PublishedEvents { get; } = new();
    public List<(string Destination, object Command, MessageContext? Context)> SentCommands { get; } = new();
    public List<Parcel> SentParcels { get; } = new();

    public FakeSendMessageBus(ILogger<FakeSendMessageBus> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<TEvent>(
        TEvent @event,
        MessageContext? context = null,
        CancellationToken cancellationToken = default)
        where TEvent : class
    {
        PublishedEvents.Add(@event!);

        _logger.LogInformation(
            "Fake publish event {eventType} with correlation {correlationId}",
            typeof(TEvent).Name,
            context?.CorrelationId);

        return Task.CompletedTask;
    }

    public Task SendCommandAsync<TCommand>(
        string destinationService,
        TCommand command,
        MessageContext? context = null,
        CancellationToken cancellationToken = default)
        where TCommand : class
    {
        SentCommands.Add((destinationService, command!, context));

        _logger.LogInformation(
            "Fake send command {commandType} to {destinationService} with correlation {correlationId}",
            typeof(TCommand).Name,
            destinationService,
            context?.CorrelationId);

        return Task.CompletedTask;
    }

    public Task SendAsync(
        Parcel parcel,
        CancellationToken cancellationToken = default)
    {
        SentParcels.Add(parcel);

        _logger.LogInformation(
            "Fake send raw parcel with id {parcelId}",
            parcel.MessageId);

        return Task.CompletedTask;
    }
}