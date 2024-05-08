namespace Base.Extensions.DependencyInjection;

public static class ChangeDatalogServiceCollectionExtensions
{
    public static IServiceCollection AddBaseHamsterChangeDatalog(this IServiceCollection services, IConfiguration configuration)
    {        
        services.Configure<ChangeDataLogHamsterOptions>(configuration);
        return services;
    }

    public static IServiceCollection AddBaseHamsterChangeDatalog(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddBaseHamsterChangeDatalog(configuration.GetSection(sectionName));
        return services;
    }

    public static IServiceCollection AddBaseHamsterChangeDatalog(this IServiceCollection services, Action<ChangeDataLogHamsterOptions> setupAction)
    {
        services.Configure(setupAction);
        return services;
    }
}