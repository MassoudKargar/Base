namespace Base.Core.ApplicationServices.Events;

public class EventDispatcherDomainExceptionHandlerDecorator(
    ILogger<EventDispatcherDomainExceptionHandlerDecorator> logger)
    : EventDispatcherDecorator
{
    private readonly ILogger<EventDispatcherDomainExceptionHandlerDecorator> _logger = logger;

    #region Fields

    public override int Order => 2;
    #endregion


    #region Publish Domain Event
    public override async Task PublishDomainEventAsync<TDomainEvent>(TDomainEvent @event)
    {
        try
        {
            await _eventDispatcher.PublishDomainEventAsync(@event);
        }
        catch (DomainStateException ex)
        {
            _logger.LogError(ZaminEventId.DomainValidationException, ex, "Processing of {EventType} With value {Event} failed at {StartDateTime} because there are domain exceptions.", @event.GetType(), @event, DateTime.Now);
        }
        catch (AggregateException ex)
        {
            if (ex.InnerException is DomainStateException domainStateException)
            {
                _logger.LogError(ZaminEventId.DomainValidationException, ex, "Processing of {EventType} With value {Event} failed at {StartDateTime} because there are domain exceptions.", @event.GetType(), @event, DateTime.Now);
            }
            throw ex;
        }
    }
    #endregion
}