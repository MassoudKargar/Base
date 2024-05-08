using Base.Extensions.DependencyInjection;
using Base.Samples.EndPoints.WebApi.Extensions.DependencyInjection.IdentityServer.Extensions;
using Serilog;

namespace Base.Samples.EndPoints.WebApi.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        IConfiguration configuration = builder.Configuration;

        builder.Services.AddSingleton<CommandDispatcherDecorator, CustomCommandDecorator>();
        builder.Services.AddSingleton<QueryDispatcherDecorator, CustomQueryDecorator>();
        builder.Services.AddSingleton<EventDispatcherDecorator, CustomEventDecorator>();

        //Base
        builder.Services.AddBaseApiCore("Base");

        //microsoft
        builder.Services.AddEndpointsApiExplorer();

        //
        builder.Services.AddBaseWebUserInfoService(configuration, "WebUserInfo", true);

        //
        builder.Services.AddBaseParrotTranslator(configuration, "ParrotTranslator");

        //
        //builder.Services.AddSoftwarePartDetector(configuration, "SoftwarePart");

        //
        builder.Services.AddNonValidatingValidator();

        //
        builder.Services.AddBaseMicrosoftSerializer();

        //
        builder.Services.AddBaseAutoMapperProfiles(option =>
        {
            option.AssemblyNamesForLoadProfiles = "Base";
        });

        //
        builder.Services.AddBaseInMemoryCaching();
        //builder.Services.AddSqlDistributedCache(configuration, "SqlDistributedCache");

        //CommandDbContext
        builder.Services.AddDbContext<SampleCommandDbContext>(
            c => c.UseSqlServer(configuration.GetConnectionString("CommandDb_ConnectionString"))
            .AddInterceptors(new SetPersianYeKeInterceptor(),
                             new AddAuditDataInterceptor()));

        //QueryDbContext
        builder.Services.AddDbContext<SampleQueryDbContext>(
            c => c.UseSqlServer(configuration.GetConnectionString("QueryDb_ConnectionString")));

        //PollingPublisher
        //builder.Services.AddPollingPublisherDalSql(configuration, "PollingPublisherSqlStore");
        //builder.Services.AddPollingPublisher(configuration, "PollingPublisher");

        //MessageInbox
        //builder.Services.AddMessageInboxDalSql(configuration, "MessageInboxSqlStore");
        //builder.Services.AddMessageInbox(configuration, "MessageInbox");

        //builder.Services.AddRabbitMqMessageBus(configuration, "RabbitMq");

        //builder.Services.AddTraceJeager(configuration, "OpenTeletmetry");

        //builder.Services.AddIdentityServer(configuration, "OAuth");

        builder.Services.AddSwagger(configuration, "Swagger");

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        //base
        app.UseBaseApiExceptionHandler();

        //Serilog
        app.UseSerilogRequestLogging();

        app.UseSwaggerUI("Swagger");

        app.UseStatusCodePages();

        app.UseCors(delegate (CorsPolicyBuilder builder)
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });

        app.UseHttpsRedirection();

        //app.Services.ReceiveEventFromRabbitMqMessageBus(new KeyValuePair<string, string>("MiniBlog", "BlogCreated"));

        var useIdentityServer = app.UseIdentityServer("OAuth");


        //app.UseAuthentication();

        if (useIdentityServer)
        {
            app.MapControllers().RequireAuthorization();
        }
        else
        {
            app.MapControllers();
        }
        //app.Services.GetService<SoftwarePartDetectorService>()?.Run();

        return app;
    }
}