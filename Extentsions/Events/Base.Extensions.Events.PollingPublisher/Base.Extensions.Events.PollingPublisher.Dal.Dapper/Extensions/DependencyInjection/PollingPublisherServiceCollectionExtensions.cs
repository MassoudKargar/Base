namespace Base.Extensions.DependencyInjection;
public static class PollingPublisherServiceCollectionExtensions
{
    public static IServiceCollection AddBasePollingPublisherDalSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PollingPublisherDalRedisOptions>(configuration);
        AddServices(services);
        return services;
    }

    public static IServiceCollection AddBasePollingPublisherDalSql(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddBasePollingPublisherDalSql(configuration.GetSection(sectionName));
        return services;
    }

    public static IServiceCollection AddBasePollingPublisherDalSql(this IServiceCollection services, Action<PollingPublisherDalRedisOptions> setupAction)
    {
        services.Configure(setupAction);
        AddServices(services);
        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IOutBoxEventItemRepository, SqlOutBoxEventItemRepository>();
    }
}