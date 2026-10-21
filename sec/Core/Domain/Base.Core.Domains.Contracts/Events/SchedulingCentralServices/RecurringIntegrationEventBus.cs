using Base.Core.Domains.Contracts.Events.SchedulingCentralServices.Events;

namespace Base.Core.Domains.Contracts.Events.SchedulingCentralServices;
public class RecurringIntegrationEventBus : IRecurringIntegrationEventBus
{
    private readonly IIntegrationEventBus _integrationEventBus;
    private readonly IIntegrationEventTopicName _integrationEventTopicName;

    public RecurringIntegrationEventBus(IIntegrationEventBus integrationEventBus,
        IIntegrationEventTopicName integrationEventTopicName)
    {
        _integrationEventBus = integrationEventBus;
        _integrationEventTopicName = integrationEventTopicName;
    }


    public async Task PublishAsync<TEvent>(string recurringJobId, TEvent eventObject, string cronExpression)
        where TEvent : IIntegrationEvent
    {
        if (string.IsNullOrEmpty(recurringJobId))
        {
            throw new ArgumentNullException(nameof(recurringJobId));
        }

        try
        {
            var cron = Cronos.CronExpression.Parse(cronExpression, Cronos.CronFormat.Standard);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"cronExpression not valid {cronExpression}", ex);
        }

        RecurringIntegrationEvent integrationEvent =
           new(_integrationEventTopicName.Calculate(eventObject), recurringJobId, cronExpression, eventObject);

        await _integrationEventBus.PublishAsync(integrationEvent);
    }

    public Task RemoveAsync(string recurringJobId)
    {
        RemoveRecurringIntegrationEvent integrationEvent = new(recurringJobId);

        return _integrationEventBus.PublishAsync(integrationEvent);
    }
}