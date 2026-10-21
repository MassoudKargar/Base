using Base.Core.Domains.Contracts.Events;

namespace Base.Core.Domains.Contracts.Events.SchedulingCentralServices.Events;
public class RecurringIntegrationEvent : IIntegrationEvent
{
    public RecurringIntegrationEvent()
    {

    }

    public RecurringIntegrationEvent(
        string fullName,
        string recurringJobId,
        string cronExpression,
        IIntegrationEvent scheduleEvent)
    {
        Name = fullName;
        RecurringJobId = recurringJobId;
        CronExpression = cronExpression;
        ScheduleEvent = System.Text.Json.JsonSerializer.Serialize((object)scheduleEvent);
    }
    public string Name { get; set; } = default!;

    public string RecurringJobId { get; set; } = default!;

    public string CronExpression { get; set; } = default!;

    public string ScheduleEvent { get; set; } = default!;
}