using Base.Core.Domains.Contracts.Events.Decoratiors;
using Base.Core.Domains.Contracts.Events.SchedulingCentralServices;

namespace Base.Core.Domains.Contracts.Events.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDefaultIntegrationEventServices(this IServiceCollection services)
    {
        services.AddScoped<IIntegrationEventTopicName, IntegrationEventTopicName>();
    }

    public static void AddCentralScheduleIntegrationEventBusServices(IServiceCollection services)
    {
        services.AddScoped<IScheduleIntegrationEventBus, CentralServiceScheduleIntegrationEventBus>();
    }

    public static void AddCentralRecurringIntegrationEventBusServices(IServiceCollection services)
    {
        services.AddScoped<IRecurringIntegrationEventBus, RecurringIntegrationEventBus>();
    }

    public static void AddIntegrationEventHandler<T, TH>(this IServiceCollection services)
        where T : DomainEvent, IIntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
    {
        //todo checking that there is only one handler per group
        services.AddTransient<TH>();
        services.AddTransient<IIntegrationEventHandler<T>>(provider =>
            new LoggingDecorator<T>(provider.GetRequiredService<TH>(), provider.GetRequiredService<ILogger<TH>>()));
        IntegrationEventDiscovery.RegisterEventType<T, TH>();
    }
}