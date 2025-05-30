using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Base.Utilities.Auth.ApiAuthentication.Options;

namespace Base.Extensions.DependencyInjection;

public static class ApiAuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddBaseApiAuthentication(this IServiceCollection services, IConfiguration configuration, string sectionName)
        => services.AddBaseApiAuthentication(configuration.GetSection(sectionName));

    public static IServiceCollection AddBaseApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiAuthenticationOption>(configuration);
        var option = configuration.Get<ApiAuthenticationOption>() ?? new();

        return services.AddAuthentication(option);
    }

    public static IServiceCollection AddBaseApiAuthentication(this IServiceCollection services, Action<ApiAuthenticationOption> action)
    { 
        services.Configure(action);
        var option = new ApiAuthenticationOption();
        action.Invoke(option);

        return services.AddAuthentication(option);
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, ApiAuthenticationOption option)
    {
        if (option.Active)
        {
            ProviderOption defaultProvider = option.DefaultProvider ?? throw new InvalidOperationException($"DefaultProvider is null");

            var authenticationBuilder = services.AddAuthentication(defaultProvider.Scheme);

            for (var i = 0; i < option.EnabledProviders.Count; i++)
            {
                ProviderOption provider = option.EnabledProviders[i];

                switch (provider.TokenTypeSupport)
                {
                    case TokenType.Jwt:
                        authenticationBuilder.AddJwtTokenSupoort(services, provider);
                        break;

                    case TokenType.Reference:
                        authenticationBuilder.AddReferenceTokenSupport(services, provider);
                        break;

                    default:
                        throw new InvalidOperationException($"Invalid token type for {provider.Scheme} ({provider.Authority})");
                }
            }

            services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes([.. option.EnabledProviders.Select(c => c.Scheme)])
                    .Build());
        }

        return services;
    }
}