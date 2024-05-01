using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Base.Extensions.Translations.Parrot.Options;
using Base.Extensions.Translations.Parrot.Services;
using Base.Extensions.Translations.Abstractions;

namespace Base.Extensions.DependencyInjection;

public static class ParrotTranslatorServiceCollectionExtensions
{
    public static IServiceCollection AddBaseParrotTranslator(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITranslator, ParrotTranslator>();
        services.Configure<ParrotTranslatorOptions>(configuration);
        return services;
    }

    public static IServiceCollection AddBaseParrotTranslator(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddBaseParrotTranslator(configuration.GetSection(sectionName));
        return services;
    }

    public static IServiceCollection AddBaseParrotTranslator(this IServiceCollection services, Action<ParrotTranslatorOptions> setupAction)
    {
        services.AddSingleton<ITranslator, ParrotTranslator>();
        services.Configure(setupAction);
        return services;
    }
}