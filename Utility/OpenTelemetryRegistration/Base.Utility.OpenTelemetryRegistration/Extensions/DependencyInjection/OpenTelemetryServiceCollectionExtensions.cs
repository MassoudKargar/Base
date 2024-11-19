namespace Base.Extensions.DependencyInjection;
public static class OpenTelemetryServiceCollectionExtensions
{
    public static WebApplicationBuilder AddObservability(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        OpenTelemetryOptions observabilityOptions = new();

        var config = configuration.GetValue<OpenTelemetryOptions>(nameof(OpenTelemetryOptions));
        if (config == null)
        {
            observabilityOptions = new OpenTelemetryOptions
            {
                ApplicationName = "Base",
                ServiceName = "OpenTelemetrySample",
                ServiceVersion = "1.0.0",
                ServiceId = "cb387bb6-9a66-444f-92b2-ff64e2a81f98",
                OltpEndpoint = "http://localhost:4317",
                ExportProcessorType = ExportProcessorType.Simple,
                SamplingProbability = 1
            };
        }
        else
        {
            configuration
                .GetRequiredSection(nameof(OpenTelemetryOptions))
                .Bind(observabilityOptions);
        }
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(observabilityOptions.ServiceName))
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

        return builder;
    }



    public static IApplicationBuilder UseBaseObservabilityMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ResponseMetricMiddleware>();
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        return app;
    }
}
