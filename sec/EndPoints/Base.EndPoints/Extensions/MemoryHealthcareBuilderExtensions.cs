using Base.EndPoints.Healthchecks;

namespace Base.EndPoints.Extensions;

internal static class MemoryHealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddMemoryHealthCheck(this IHealthChecksBuilder healthChecksBuilder)
    {
        healthChecksBuilder.AddCheck<SystemMemoryHealthcheck>("Memory");

        return healthChecksBuilder;
    }
}