namespace Base.Extensions.DependencyInjection;

public static class PollingPublisherServiceCollectionExtensions
{
    public static IServiceCollection AddBasePollingPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PollingPublisherOptions>(configuration);
        AddServices(services);
        return services;
    }

    public static IServiceCollection AddBasePollingPublisher(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddBasePollingPublisher(configuration.GetSection(sectionName));
        return services;
    }

    public static IServiceCollection AddBasePollingPublisher(this IServiceCollection services, Action<PollingPublisherOptions> setupAction)
    {
        services.Configure(setupAction);
        AddServices(services);
        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddHostedService<PoolingPublisherBackgroundService>();
    }
}