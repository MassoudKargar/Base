using Base.EndPoints.ConfigurationViewers;
using Base.EndPoints.Healthchecks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Base.EndPoints.Web.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseHerisFramework(this WebApplication app)
    {
        app.UseApiConsumerChecker();
        app.UseExceptionAdapter();
        app.UseConfigurationViewer();
        return app;
    }

    public static void AddDefaultHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

        var sqlServerConnection = configuration["ConnectionStrings:SqlServerConnectionString"];
        if (!string.IsNullOrEmpty(sqlServerConnection))
        {
            healthChecksBuilder
                .AddSqlServer(sqlServerConnection,
                    name: "sqlserverdb-check",
                    tags: new[] { "sqlserverdb" },
                    failureStatus: HealthStatus.Degraded);
        }

        var redisConnection = configuration["ConnectionStrings:RedisConnectionString"];
        if (!string.IsNullOrEmpty(redisConnection))
        {
            healthChecksBuilder
                .AddRedis(redisConnection,
                    name: "redis-check",
                    tags: new[] { "redis" },
                    failureStatus: HealthStatus.Degraded);
        }
        services.AddHttpClient<HealthCheckPublisher>();
        services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();
    }
}
