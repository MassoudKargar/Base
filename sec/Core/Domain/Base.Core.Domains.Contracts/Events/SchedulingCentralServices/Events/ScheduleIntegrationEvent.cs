namespace Base.Core.Domains.Contracts.Events.SchedulingCentralServices.Events;
public class ScheduleIntegrationEvent : IIntegrationEvent
{
    public ScheduleIntegrationEvent()
    {

    }

    public ScheduleIntegrationEvent(string fullName,
        DateTimeOffset publishTime,
        IIntegrationEvent scheduleEvent)
    {
        Name = fullName;
        PublishTime = publishTime;
        ScheduleEvent = System.Text.Json.JsonSerializer.Serialize((object)scheduleEvent);
    }
    public string Name { get; set; } = default!;

    public DateTimeOffset PublishTime { get; set; }

    public string ScheduleEvent { get; set; } = default!;
}