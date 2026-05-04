namespace Base.Infrastructure.Persistence.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{

    public static void AddDataEntityFrameworkServices(this IServiceCollection services)
    {
        services.AddUnitOfWorkByEntityFramework();

        services.AddDomainEventsDispatcherByEntityFramework();
    }


    private static IServiceCollection AddDomainEventsDispatcherByEntityFramework(this IServiceCollection services)
    {
        services.AddTransient<IDomainEventDetector, DomainEventDetector>();
        services.AddTransient<IIntegrationEventDetector, DomainEventDetector>();
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
        return services;
    }

    private static IServiceCollection AddUnitOfWorkByEntityFramework(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Disbale for save not working Bug
        //services.AddScoped<IManualUnitOfWork, ManualUnitOfWork>();
        return services;
    }
}
