namespace Base.Extensions.DependencyInjection;
public static class SerilogServiceCollectionExtensions
{
    public static WebApplicationBuilder AddBaseSerilog(this WebApplicationBuilder builder, IConfiguration configuration, params Type[] enrichersType)
    {

        builder.Services.Configure<SerilogApplicationEnricherOptions>(configuration);
        return AddServices(builder, enrichersType);
    }

    public static WebApplicationBuilder AddBaseSerilog(this WebApplicationBuilder builder, IConfiguration configuration, string sectionName, params Type[] enrichersType)
    {
        return builder.AddBaseSerilog(configuration.GetSection(sectionName), enrichersType);
    }

    public static WebApplicationBuilder AddBaseSerilog(this WebApplicationBuilder builder, Action<SerilogApplicationEnricherOptions> setupAction, params Type[] enrichersType)
    {
        builder.Services.Configure(setupAction);
        return AddServices(builder, enrichersType);
    }

    private static WebApplicationBuilder AddServices(WebApplicationBuilder builder, params Type[] enrichersType)
    {

        List<ILogEventEnricher> logEventEnrichers = new();

        builder.Services.AddTransient<BaseUserInfoEnricher>();
        builder.Services.AddTransient<BaseApplicationEnricher>();
        foreach (var enricherType in enrichersType)
        {
            builder.Services.AddTransient(enricherType);
        }
        
        builder.Host.UseSerilog((ctx, services, lc) => {
            logEventEnrichers.Add(services.GetRequiredService<BaseUserInfoEnricher>());
            logEventEnrichers.Add(services.GetRequiredService<BaseApplicationEnricher>());
            foreach (var enricherType in enrichersType)
            {
                logEventEnrichers.Add(services.GetRequiredService(enricherType) as ILogEventEnricher);
            }

            lc
            //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .Enrich.FromLogContext()
            .Enrich.With([.. logEventEnrichers])
            .Enrich.WithExceptionDetails()
            .Enrich.WithSpan()
            .ReadFrom.Configuration(ctx.Configuration);
        });
        return builder;
    }
}
