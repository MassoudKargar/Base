using Base.Core.Domains.Contracts.Events;

namespace Base.Core.Domains.Contracts.Events.SchedulingCentralServices.Events;
public class RemoveRecurringIntegrationEvent : IIntegrationEvent
{
    public RemoveRecurringIntegrationEvent()
    {

    }

    public RemoveRecurringIntegrationEvent(string recurringJobId)
    {
        RecurringJobId = recurringJobId;
    }

    public string RecurringJobId { get; set; } = default!;

}