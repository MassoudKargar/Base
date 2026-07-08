using Base.EndPoints.Web.Extensions.Swaggers.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace Base.EndPoints.Web.Extensions.Swaggers.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiDocumentation(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
    {
        var option = configuration
            .GetSection(sectionName)
            .Get<SwaggerOption>();

        if (option is not { Enabled: true })
            return services;

        services.AddOpenApi(options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = option.SwaggerDoc.Title,
                    Version = option.SwaggerDoc.Version
                };

                var oauth = option.OAuth;

                if (oauth.Enabled)
                {
                    document.Components ??= new();

                    document.Components.SecuritySchemes?["OAuth2"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(oauth.AuthorizationUrl),
                                TokenUrl = new Uri(oauth.TokenUrl),
                                Scopes = oauth.Scopes
                            }
                        }
                    };
                }

                return Task.CompletedTask;
            });
        });

        return services;
    }

    public static WebApplication UseOpenApiDocumentation(
        this WebApplication app,
        string sectionName)
    {
        var option = app.Configuration
            .GetSection(sectionName)
            .Get<SwaggerOption>();

        if (option is not { Enabled: true })
            return app;

        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        {
            options.Title = option.SwaggerDoc.Title;
            options.Theme = ScalarTheme.BluePlanet;
            options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);

            if (option.OAuth.Enabled)
            {
                options.WithTitle("Medication Schedule API")
                    .WithTheme(ScalarTheme.DeepSpace)
                    .ShowOperationId()
                    .ExpandAllTags()
                    .SortTagsAlphabetically()
                    .SortOperationsByMethod()
                    .PreserveSchemaPropertyOrder()
                    .AddAuthorizationCodeFlow(
                        "OAuth2",
                        flow =>
                        {
                            flow.AuthorizationUrl = option.OAuth.AuthorizationUrl;
                            flow.TokenUrl = option.OAuth.TokenUrl;
                        });
            }
        });

        return app;
    }
}