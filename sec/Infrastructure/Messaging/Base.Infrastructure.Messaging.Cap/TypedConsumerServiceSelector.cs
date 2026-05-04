using System.Reflection;
using Base.Core.Domains.Contracts.Events;

namespace Base.Infrastructure.Messaging.Cap;

public class TypedConsumerServiceSelector : ConsumerServiceSelector
{
    private readonly CapOptions _capOptions;
    private readonly IServiceCollection serviceDescriptors;

    public TypedConsumerServiceSelector(IServiceProvider serviceProvider, IServiceCollection serviceDescriptors)
        : base(serviceProvider)
    {
        _capOptions = serviceProvider.GetRequiredService<IOptions<CapOptions>>().Value;
        this.serviceDescriptors = serviceDescriptors;
    }

    protected override IEnumerable<ConsumerExecutorDescriptor> FindConsumersFromInterfaceTypes(IServiceProvider provider)
    {
        var executorDescriptorList = new List<ConsumerExecutorDescriptor>();

        var consumerHandlers = serviceDescriptors
              .Where(c => FilterHandlers(c.ServiceType))
              .Select(c => c.ImplementationType)
              .ToList();


        foreach (var handlerType in consumerHandlers)
        {
            if (handlerType != null)
                executorDescriptorList.AddRange(GetMyDescription(handlerType.GetTypeInfo()));
        }

        //TODO: exception when already even name subscribed
        //      current status : ignore duplicate event!
        return executorDescriptorList;
    }

    private static bool FilterHandlers(Type t)
    {
        return t.GetInterfaces()
            .Any(d => d.IsGenericType && d.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
            && t.IsClass
            && !t.IsAbstract;
    }

    private IEnumerable<ConsumerExecutorDescriptor> GetMyDescription(TypeInfo typeInfo)
    {
        //TODO: Refactoring This Method
        var methods = typeInfo.DeclaredMethods.Where(x => x.Name == "HandleAsync")
            .ToList();

        methods.AddRange(typeInfo.BaseType?.GetTypeInfo().DeclaredMethods.Where(x => x.Name == "HandleAsync")
            .ToList());



        var groupType = GetGroupType(typeInfo);
        foreach (var method in methods)
        {
            if (method == null) throw new NullReferenceException(nameof(method));
            var topic = GetTopic(groupType);
            if (!string.IsNullOrEmpty(_capOptions.GroupNamePrefix))
                topic = $"{_capOptions.GroupNamePrefix}.{topic}";

            // TODO use TEvent
            var parameters = method.GetParameters().Select(p => new ParameterDescriptor
            {
                Name = p.Name,
                ParameterType = p.ParameterType,
                IsFromCap = p.GetCustomAttributes(typeof(FromCapAttribute)).Any(),

            }).ToList();

            string capName;

            var originalEventFullNameAttribute = parameters.FirstOrDefault(x => !x.IsFromCap)?.ParameterType.GetCustomAttribute<OriginalEventFullNameAttribute>(); ;
            if (originalEventFullNameAttribute != null)
            {
                capName = originalEventFullNameAttribute.Name;
            }
            else
            {
                capName = parameters.FirstOrDefault(x => !x.IsFromCap)?.ParameterType.FullName;
            }


            if (string.IsNullOrEmpty(capName))
                throw new NullReferenceException(nameof(capName));

            yield return new ConsumerExecutorDescriptor
            {
                Attribute = new CapSubscribeAttribute(capName)
                {
                    Group = topic
                },
                Parameters = parameters,
                MethodInfo = method,
                ImplTypeInfo = typeInfo,
                TopicNamePrefix = _capOptions.TopicNamePrefix,
                ServiceTypeInfo = typeInfo
            };
        }
    }

    private string GetTopic(string? groupType)
    {
        if (string.IsNullOrEmpty(groupType))
            return $"{_capOptions.DefaultGroupName}.{_capOptions.Version}";
        return $"{_capOptions.DefaultGroupName}.{groupType.ToLower()}.{_capOptions.Version}";
    }

    private static string? GetGroupType(TypeInfo typeInfo)
    {
        var groupEventNameAttribute = typeInfo.GetCustomAttribute<GroupEventNameAttribute>(true);
        if (groupEventNameAttribute?.GroupType == GroupType.Default.ToString())
            return null;

        return groupEventNameAttribute?.GroupType;
    }
}
