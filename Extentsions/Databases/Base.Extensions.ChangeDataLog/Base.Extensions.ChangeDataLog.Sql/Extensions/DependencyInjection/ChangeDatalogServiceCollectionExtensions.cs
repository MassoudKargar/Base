namespace Base.Extensions.DependencyInjection;

public static class ChangeDatalogServiceCollectionExtensions
{
    public static IServiceCollection AddBaseChangeDatalogDalSql(this IServiceCollection services, IConfiguration configuration)
    {        
        services.AddScoped<IEntityChangeInterceptorItemRepository, DapperEntityChangeInterceptorItemRepository>();
        services.Configure<ChangeDataLogSqlOptions>(configuration);
        return services;
    }

    public static IServiceCollection AddBaseChangeDatalogDalSql(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddBaseChangeDatalogDalSql(configuration.GetSection(sectionName));
        return services;
    }

    public static IServiceCollection AddBaseChangeDatalogDalSql(this IServiceCollection services, Action<ChangeDataLogSqlOptions> setupAction)
    {
        services.AddScoped<IEntityChangeInterceptorItemRepository, DapperEntityChangeInterceptorItemRepository>();
        services.Configure(setupAction);
        return services;
    }
}