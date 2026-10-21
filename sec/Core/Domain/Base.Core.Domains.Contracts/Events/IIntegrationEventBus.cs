namespace Base.Core.Domains.Contracts.Events;

public interface IIntegrationEventBus
{
    public Task PublishAsync<TEvent>(TEvent eventObject, string? correlationId = null)
        where TEvent : IIntegrationEvent;

    /// <summary>
    /// Publish by topic name
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="name">the topic name or exchange router key.</param>
    /// <param name="eventObject">message body content, that will be serialized. </param>
    /// <returns></returns>
    public Task PublishAsync(string name, object eventObject);
    Task PublishAsync<TEvent>(TEvent eventObject) where TEvent : IIntegrationEvent;
}