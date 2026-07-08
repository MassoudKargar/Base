namespace Base.Extensions.DependencyInjection;

public static class OpenTelemetryServiceCollectionExtensions
{
    public static WebApplicationBuilder AddObservability(
        this WebApplicationBuilder builder,
        OpenTelemetryOptions? options = null)
    {
        var observabilityOptions = options ?? new OpenTelemetryOptions();

        var section = builder.Configuration.GetSection(nameof(OpenTelemetryOptions));

        if (section.Exists())
        {
            section.Bind(observabilityOptions);
        }

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource =>
                resource.AddService(observabilityOptions.ServiceName))
            .AddMetrics(observabilityOptions)
            .AddTracing(observabilityOptions)
            .AddLogging(observabilityOptions);

        return builder;
    }

    private static OpenTelemetryBuilder AddLogging(this OpenTelemetryBuilder builder, OpenTelemetryOptions observabilityOptions)
    {

        builder.WithLogging(logging =>
        {
            logging
            .AddOtlpExporter(_ =>
            {
                _.Endpoint = new Uri(observabilityOptions.OltpEndpoint);
                _.ExportProcessorType = ExportProcessorType.Batch;
                _.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            });
        });
        return builder;

    }




    private static OpenTelemetryBuilder AddTracing(this OpenTelemetryBuilder builder, OpenTelemetryOptions observabilityOptions)
    {

        builder.WithTracing(tracing =>
        {
            string serviceName = $"{observabilityOptions.ApplicationName}.{observabilityOptions.ServiceName}";

            tracing
                 .AddSource("*")

            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion: observabilityOptions.ServiceVersion, serviceInstanceId: observabilityOptions.ServiceId))
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
            })
            .AddSqlClientInstrumentation()
                .SetErrorStatusOnException()

            .AddEntityFrameworkCoreInstrumentation()
            .SetSampler(new TraceIdRatioBasedSampler(observabilityOptions.SamplingProbability))
            .AddOtlpExporter(oltpOptions =>
            {
                oltpOptions.Endpoint = new Uri(observabilityOptions.OltpEndpoint);
                oltpOptions.ExportProcessorType = observabilityOptions.ExportProcessorType;
                oltpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            });

        });

        return builder;
    }

    private static OpenTelemetryBuilder AddMetrics(this OpenTelemetryBuilder builder, OpenTelemetryOptions observabilityOptions)
    {
        builder.WithMetrics(metrics =>
        {
            metrics
            .AddMeter("*")
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddHttpClientInstrumentation();
            metrics
                .AddOtlpExporter((_, metricReaderOptions) =>
                {

                    _.Endpoint = new Uri(observabilityOptions.OltpEndpoint);
                    _.ExportProcessorType = ExportProcessorType.Batch;
                    _.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 5000;

                });
        });
        builder.Services.AddScoped<MetricReporter>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenTelemetryOptions>>().Value;
            return new MetricReporter(options.ServiceName, "http");
        });
        return builder;
    }



    public static IApplicationBuilder UseBaseObservabilityMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ResponseMetricMiddleware>();
        return app;
    }
}
