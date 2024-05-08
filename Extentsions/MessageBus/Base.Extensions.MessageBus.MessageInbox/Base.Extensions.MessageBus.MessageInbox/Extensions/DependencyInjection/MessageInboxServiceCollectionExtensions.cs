﻿namespace Base.Extensions.DependencyInjection;

public static class MessageInboxServiceCollectionExtensions
{
    public static IServiceCollection AddBaseMessageInbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessageInboxOptions>(configuration);
        AddServices(services);
        return services;
    }

    public static IServiceCollection AddBaseMessageInbox(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddBaseMessageInbox(configuration.GetSection(sectionName));
        return services;
    }

    public static IServiceCollection AddBaseMessageInbox(this IServiceCollection services, Action<MessageInboxOptions> setupAction)
    {
        services.Configure(setupAction);
        AddServices(services);
        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IMessageConsumer, InboxMessageConsumer>();
    }
}