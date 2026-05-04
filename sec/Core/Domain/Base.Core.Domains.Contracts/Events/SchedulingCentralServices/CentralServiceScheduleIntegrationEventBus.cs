using Base.Core.Domains.Contracts.Events.SchedulingCentralServices.Events;

namespace Base.Core.Domains.Contracts.Events.SchedulingCentralServices;
internal class CentralServiceScheduleIntegrationEventBus : IScheduleIntegrationEventBus
{
    private readonly IIntegrationEventBus _integrationEventBus;
    private readonly IIntegrationEventTopicName _integrationEventTopicName;

    public CentralServiceScheduleIntegrationEventBus(IIntegrationEventBus integrationEventBus,
        IIntegrationEventTopicName integrationEventTopicName)
    {
        _integrationEventBus = integrationEventBus;
        _integrationEventTopicName = integrationEventTopicName;
    }

    public async Task PublishAsync<TEvent>(TEvent eventObject, DateTimeOffset publishDate)
        where TEvent : IIntegrationEvent
    {
        ScheduleIntegrationEvent integrationEvent =
            new(_integrationEventTopicName.Calculate(eventObject), publishDate, eventObject);

        await _integrationEventBus.PublishAsync(integrationEvent);
    }
}