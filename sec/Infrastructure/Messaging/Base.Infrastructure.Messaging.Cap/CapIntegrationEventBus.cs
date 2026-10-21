using Base.Core.Domains.Contracts.Events;

namespace Base.Infrastructure.Messaging.Cap;
internal class CapIntegrationEventBus : IIntegrationEventBus
{
    private readonly ICapPublisher _capPublisher;
    private readonly IIntegrationEventTopicName _integrationEventTopicName;
    public CapIntegrationEventBus(ICapPublisher capPublisher, IIntegrationEventTopicName integrationEventTopicName)
    {
        _capPublisher = capPublisher;
        _integrationEventTopicName = integrationEventTopicName;
    }

    // 1. Publisher: await eventBus.PublishAsync(eventObject, Activity.Current.TraceId.ToString())
    // 2. Subscriber: using var scope = logger.BeginScope("Processing message for trace {TraceId}", header.CorrelationId); -- [FromCap]CapHeader header
    public Task PublishAsync<TEvent>(TEvent eventObject, string? correlationId) where TEvent : IIntegrationEvent
    {
        Dictionary<string, string?> headers = new();
        headers.Add(Headers.CorrelationId, !string.IsNullOrWhiteSpace(correlationId) ? correlationId : Guid.NewGuid().ToString());

        var correlationIdProperty = eventObject.GetType().GetProperty("CorrelationId");
        if (correlationIdProperty != null)
            correlationIdProperty.SetValue(eventObject, correlationId, null);
        return _capPublisher.PublishAsync(_integrationEventTopicName.Calculate(eventObject), eventObject, headers);
    }

    public Task PublishAsync<TEvent>(TEvent eventObject) where TEvent : IIntegrationEvent
    {
        return PublishAsync(eventObject, null);
    }

    public Task PublishAsync(string name, object eventObject)
    {
        return _capPublisher.PublishAsync(name, eventObject);
    }
}