namespace Base.EndPoints.Web.Extensions.DependencyInjection;
public static class AddBaseServicesExtensions
{
    public static IServiceCollection AddBaseUtilityServices(
        this IServiceCollection services)
    {
        services.AddTransient<BaseServices>();
        return services;
    }
}