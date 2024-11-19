namespace Base.Extensions.DependencyInjection;
public static class OpenTelemetryServiceCollectionExtensions
{
    public static IServiceCollection AddBaseObservabilitySupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenTelemetryOptions>(configuration);
        RegisterTraceServices(services);
        services.RegisterMetricService();
        return services;
    }
    public static IServiceCollection AddBaseObservabilitySupport(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddBaseObservabilitySupport(configuration.GetSection(sectionName));
        return services;
    }
    public static IServiceCollection AddBaseObservabilitySupport(this IServiceCollection services, Action<OpenTelemetryOptions> setupAction)
    {
        services.Configure(setupAction);
        RegisterTraceServices(services);
        services.RegisterMetricService();
        RegisterLoggingService(services);
        return services;
    }

    private static IServiceCollection RegisterLoggingService(IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        OpenTelemetryOptions value = provider.GetRequiredService<IOptions<OpenTelemetryOptions>>().Value;
        services.AddOpenTelemetry().WithLogging();
        return services;
    }
    private static IServiceCollection RegisterTraceServices(IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        OpenTelemetryOptions options = provider.GetRequiredService<IOptions<OpenTelemetryOptions>>().Value;
        services.AddOpenTelemetry().WithTracing(delegate (TracerProviderBuilder tracerProviderBuilder)
        {
            string serviceName = options.ApplicationName + "." + options.ServiceName;
            tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName, null, options.ServiceVersion, autoGenerateServiceInstanceId: true, options.ServiceId))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddSqlClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .SetSampler(new TraceIdRatioBasedSampler(options.SamplingProbability))
                .AddOtlpExporter(oltpOptions => 
                {
                    oltpOptions.Endpoint = new Uri(options.OltpEndpoint);
                    oltpOptions.ExportProcessorType = options.ExportProcessorType;
                });
        });
        return services;
    }

    public static IServiceCollection RegisterMetricService(this IServiceCollection services)
    {
        ServiceProvider provider = services.BuildServiceProvider();
        OpenTelemetryOptions options = provider.GetRequiredService<IOptions<OpenTelemetryOptions>>().Value;
        services.AddOpenTelemetry().WithMetrics(delegate (MeterProviderBuilder opts)
        {
            opts.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(options.ApplicationName)).AddMeter(options.ApplicationName).AddRuntimeInstrumentation()
                .AddOtlpExporter(oltpOptions =>
                {
                    oltpOptions.Endpoint = new Uri(options.OltpEndpoint);
                    oltpOptions.ExportProcessorType = options.ExportProcessorType;
                });
        });
        services.AddSingleton(new MetricReporter(options.ApplicationName, options.ServiceName));
        return services;
    }

    public static IApplicationBuilder UseBaseObservabilityMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ResponseMetricMiddleware>();
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        return app;
    }
}
