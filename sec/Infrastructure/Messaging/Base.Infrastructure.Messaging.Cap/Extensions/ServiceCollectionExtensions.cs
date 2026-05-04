using Base.Core.Domains.Contracts.Events;
using Base.Core.Domains.Contracts.UnitOfWorks;
using Base.Extensions.Messaging;

namespace Base.Infrastructure.Messaging.Cap.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCapMessagingFrameworkServices(this IServiceCollection services, MessagingOptions options)
    {
        Common.Schema = options.Schema;
        CustomDataStorage._disableRetriesForReceivedMessages = options.DisableRetriesForReceivedMessages;
        services.AddScoped<IIntegrationEventBus, CapIntegrationEventBus>();
        services.AddSingleton<IConsumerServiceSelector, TypedConsumerServiceSelector>();
        services.AddScoped<IUnitOfWorkInterceptor, CapUnitOfWorkInterceptor>();

        const string dashboardAuthorizationPolicy = "DashboardAuthorizationPolicy";
        const string dashboardAuthenticationSchemes = "CapBasicAuthentication";
        services
           .AddAuthorization(options =>
           {
               options.AddPolicy(dashboardAuthorizationPolicy, policy => policy
                   .AddAuthenticationSchemes(dashboardAuthenticationSchemes)
                   .RequireAuthenticatedUser());
           })
           .AddAuthentication()
           .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(dashboardAuthenticationSchemes, null);

        services.AddCap(x =>
            {
                switch (options.Type)
                {
                    case TransportType.InMemory:
                        x.UseInMemoryMessageQueue();
                        break;
                    case TransportType.Kafka:
                        x.UseKafka(s =>
                        {
                            s.Servers = options.Server;
                        });
                        break;
                    case TransportType.RabbitMq:
                        x.UseRabbitMQ(s =>
                        {
                            // use new header from Cap: cap-exec-instance-id
                            //s.CustomHeadersBuilder = (e, d) => new List<KeyValuePair<string, string>>
                            //{
                            //   new KeyValuePair<string, string>("hit-received-machine-name", Environment.MachineName),
                            //};

                            s.ConnectionFactoryOptions = opt =>
                            {
                                opt.Uri = new Uri(options.Server);

                                if (options.UsageMode != null)
                                {
                                    opt.ClientProvidedName += $"[{options.UsageMode.Trim()}]";
                                    opt.ClientProperties.Add("UsageMode", options.UsageMode.Trim());
                                }
                            };

                            if (options.PrefetchCount.HasValue)
                                s.BasicQosOptions = new RabbitMQOptions.BasicQos(options.PrefetchCount.Value);

                        });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(options.Type));
                }

                if (options.Dashboard)
                {
                    BasicAuthenticationHandler.Username = options.DashboardUsername ?? "";
                    BasicAuthenticationHandler.Password = options.DashboardPassword ?? "";

                    x.UseDashboard(option =>
                    {
                        option.PathBase = options.PathBase;
                        option.PathMatch = options.PathMatch;
                        option.AuthorizationPolicy = dashboardAuthorizationPolicy;
                        option.AllowAnonymousExplicit = false;
                        //option.
                        //option.UseAuth = true;
                        //option.DefaultAuthenticationScheme = "CapBasicAuthentication";
                    });
                }

                x.UsePostgreSql(ConfigureSqOptionalServer(options));
                x.SucceedMessageExpiredAfter = (int)options.SucceedMessageExpiredAfter.TotalSeconds;
                x.FailedMessageExpiredAfter = (int)options.FailedMessageExpiredAfter.TotalSeconds;
                x.UseStorageLock = options.ConcurrentLock;
                x.FailedRetryInterval = (int)options.FailedRetryInterval.TotalSeconds;
                x.FallbackWindowLookbackSeconds = (int)options.FallbackWindowLookBack.TotalSeconds;
            })
            .AddSubscribeFilter<CapSubscribeFilter>();


        if (options.OpenTelemetryTracing)
        {

            services.AddOpenTelemetry()
                  .WithTracing(builder => builder.AddCapInstrumentation());

        }

        ReplaceDataStorage(services);

        services.AddSingleton<IStorageInitializer, EntityFrameworkStorageInitializer>();
    }

    //[Obsolete("All options must be passed with [options] parameters.")]
    //public static void AddCapMessagingFrameworkServices(this IServiceCollection services,
    //     MessagingOptions options,
    //     string schema = "messaging",
    //     string? pathBase = null,
    //     TimeSpan? succeedMessageExpiredAfter = null,
    //     TimeSpan? failedMessageExpiredAfter = null)
    //{
    //    options.Schema = schema;

    //    if (pathBase != null)
    //        options.PathBase = pathBase;

    //    if (failedMessageExpiredAfter.HasValue)
    //        options.FailedMessageExpiredAfter = failedMessageExpiredAfter.Value;

    //    if (succeedMessageExpiredAfter.HasValue)
    //        options.SucceedMessageExpiredAfter = succeedMessageExpiredAfter.Value;

    //    services.AddCapMessagingFrameworkServices(options);
    //}

    private static void ReplaceDataStorage(IServiceCollection services)
    {
        var serviceDescriptor = services
            .First(descriptor => descriptor.ServiceType == typeof(IDataStorage));

        services.Remove(serviceDescriptor);
        services.AddSingleton<IDataStorage, CustomDataStorage>();
        services.AddSingleton<PostgreSqlDataStorage>();

       

    }

    private static Action<PostgreSqlOptions> ConfigureSqOptionalServer(MessagingOptions options)
    {
        return configure =>
        {
            //configure.DataSource = NpgsqlDataSource.Create(options.PostgresSqlConnection);
            configure.ConnectionString = options.PostgresSqlConnection;
            configure.Schema = options.Schema;
        };
    }

}