namespace Base.Core.Domains.Contracts.Events.Decoratiors;
public class LoggingDecorator<T> : IIntegrationEventHandler<T> where T : class, IIntegrationEvent
{
    private readonly IIntegrationEventHandler<T> _sampleEventHandler;
    private readonly ILogger<IIntegrationEventHandler<T>> _logger;

    public LoggingDecorator(IIntegrationEventHandler<T> sampleEventHandler, ILogger<IIntegrationEventHandler<T>> logger)
    {
        _sampleEventHandler = sampleEventHandler;
        _logger = logger;
    }
    public async Task HandleAsync(T eventToHandle)
    {
        var corellationId = eventToHandle.GetType().GetRuntimeProperty("CorrelationId")?.GetValue(eventToHandle);
        using IDisposable scope = _logger.BeginScope("Handling message with traceId: {TraceId}", corellationId);
        await _sampleEventHandler.HandleAsync(eventToHandle);
        _logger.LogInformation("Message handled successfully.", eventToHandle.GetType().Name);
    }
}
