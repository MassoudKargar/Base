using Base.EndPoints.Healthchecks;

namespace Base.EndPoints.Extensions;

internal static class HealthCheckServiceCollectionExtensions
{
    public static IHealthChecksBuilder AddDefaultHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();
        services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();
        return healthChecksBuilder;
    }
}