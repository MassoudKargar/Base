using System.Reflection.Metadata;

namespace Base.Core.Domains.Contracts.Events;
public static class IntegrationEventDiscovery
{
    private static List<(Type EventType, Type EventTypeHandler)> EventTypes = new List<(Type EventType, Type EventTypeHandler)>();

    internal static void RegisterEventType<T, TH>()
        where T : DomainEvent, IIntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
    {
        EventTypes.Add((typeof(T), typeof(TH)));
    }

    public static IEnumerable<SubscribedIntegrationEventInfo> GetSubscribedIntegrationEvents()
    {
        return EventTypes
            .Select(ExtractEventData)
            .OrderBy(c => c.SubscribedName)
            .ToList();
    }

    public static IEnumerable<IntegrationEventInfo> GetIntegrationEvents()
    {
        var eventList = Assembly
        .GetEntryAssembly()
        .GetReferencedAssemblies()
        .Select(Assembly.Load)
        .SelectMany(x => x.DefinedTypes)
        .Where(type => typeof(IIntegrationEvent).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);


        return eventList
                .Select(ExtractEventInfo)
                .OrderBy(c => c.FullName)
                .ToList();
    }

    private static IntegrationEventInfo ExtractEventInfo(Type type)
    {
        var customeAttribute = type.GetCustomAttribute<OriginalEventFullNameAttribute>();

        return new IntegrationEventInfo
        {
            Assembly = type.Assembly.GetName().ToString(),
            FullName = type.FullName,
            SubscribedName = customeAttribute is not null ? customeAttribute.Name : type.FullName,
            EntryAssembly = Assembly.GetEntryAssembly()?.GetName().ToString(),
        };
    }

    private static SubscribedIntegrationEventInfo ExtractEventData((Type EventType, Type EventTypeHandler) type)
    {
        var customeAttribute = type.EventType.GetCustomAttribute<OriginalEventFullNameAttribute>();
        var groupAttribute = type.EventTypeHandler?.GetCustomAttribute<GroupEventNameAttribute>();

        return new SubscribedIntegrationEventInfo
        {
            Assembly = type.EventType.Assembly.GetName().ToString(),
            FullName = type.EventType.FullName,
            SubscribedName = customeAttribute is not null ? customeAttribute.Name : type.EventType.FullName,
            EntryAssembly = Assembly.GetEntryAssembly()?.GetName().ToString(),
            GroupName = groupAttribute?.GroupType
        };
    }

}

public class IntegrationEventInfo
{
    public string SubscribedName { get; set; }

    public string FullName { get; set; }

    public string Assembly { get; set; }

    public string EntryAssembly { get; set; }

    public bool HaveOriginalName => FullName != SubscribedName;

}

public class SubscribedIntegrationEventInfo : IntegrationEventInfo
{
    public string? GroupName { get; internal set; }
}