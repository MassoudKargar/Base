using Base.EndPoints.Web.Extensions.Swaggers.Options;
using Microsoft.AspNetCore.Authorization;
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
        var option = ReadOptions(configuration, sectionName);
        if (!option.Enabled)
            return services;

        services.AddEndpointsApiExplorer();
        services.AddOpenApi(option.DocumentName, options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = option.Document.Title,
                    Version = option.Document.Version,
                    Description = option.Document.Description
                };

                if (option.OAuth.Enabled || option.Bearer.Enabled)
                {
                    document.Components ??= new();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                }

                if (option.OAuth.Enabled)
                {
                    document.Components!.SecuritySchemes!["OAuth2"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(option.OAuth.AuthorizationUrl),
                                TokenUrl = new Uri(option.OAuth.TokenUrl),
                                Scopes = option.OAuth.Scopes
                            }
                        }
                    };
                }

                if (option.Bearer.Enabled)
                {
                    document.Components!.SecuritySchemes!["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = option.Bearer.Description
                    };
                }
                // References created during operation transforms need their owning document
                // before OpenAPI.NET can serialize security requirement keys.
                foreach (var path in document.Paths.Values)
                {
                    if (path.Operations is null)
                        continue;
                    foreach (var operation in path.Operations.Values)
                    {
                        if (operation.Security is null)
                            continue;
                        foreach (var requirement in operation.Security)
                        {
                            var entries = requirement.ToArray();
                            requirement.Clear();
                            foreach (var entry in entries)
                                requirement[new OpenApiSecuritySchemeReference(entry.Key.Reference.Id, document)] = entry.Value;
                        }
                    }
                }
                return Task.CompletedTask;
            });

            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                var metadata = context.Description.ActionDescriptor.EndpointMetadata;
                if (metadata.OfType<IAllowAnonymous>().Any() || !metadata.OfType<IAuthorizeData>().Any())
                    return Task.CompletedTask;

                operation.Security ??= [];
                if (option.OAuth.Enabled)
                    operation.Security.Add(new OpenApiSecurityRequirement { [new OpenApiSecuritySchemeReference("OAuth2")] = [] });
                if (option.Bearer.Enabled)
                    operation.Security.Add(new OpenApiSecurityRequirement { [new OpenApiSecuritySchemeReference("Bearer")] = [] });
                operation.Responses ??= new();
                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
                return Task.CompletedTask;
            });
        });
        return services;
    }

    public static WebApplication UseOpenApiDocumentation(
        this WebApplication app,
        string sectionName)
    {
        var option = ReadOptions(app.Configuration, sectionName);
        if (!option.Enabled)
            return app;

        app.MapOpenApi(option.DocumentRoutePattern);
        app.MapScalarApiReference(option.ScalarRoute, options =>
        {
            options.WithTitle(option.Document.Title)
                .WithTheme(ScalarTheme.BluePlanet)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .WithOpenApiRoutePattern(option.DocumentRoutePattern)
                .AddDocument(option.DocumentName, option.Document.Title)
                .ShowOperationId()
                .SortTagsAlphabetically();

            if (option.OAuth.Enabled)
            {
                options.AddPreferredSecuritySchemes("OAuth2")
                    .AddDefaultScopes("OAuth2", option.OAuth.Scopes.Keys)
                    .AddAuthorizationCodeFlow("OAuth2", flow =>
                    {
                        flow.AuthorizationUrl = option.OAuth.AuthorizationUrl;
                        flow.TokenUrl = option.OAuth.TokenUrl;
                        flow.ClientId = option.OAuth.ClientId;
                        flow.Pkce = option.OAuth.UsePkce ? Pkce.Sha256 : null;
                    });
            }
            else if (option.Bearer.Enabled)
            {
                options.AddPreferredSecuritySchemes("Bearer");
            }
        });
        return app;
    }

    private static SwaggerOption ReadOptions(IConfiguration configuration, string sectionName)
    {
        var option = configuration.GetSection(sectionName).Get<SwaggerOption>() ?? new() { Enabled = false };
        if (!option.Enabled)
            return option;
        if (string.IsNullOrWhiteSpace(option.DocumentName))
            throw new InvalidOperationException($"{sectionName}:DocumentName must not be empty.");
        if (!option.DocumentRoutePattern.StartsWith('/') || !option.DocumentRoutePattern.Contains("{documentName}"))
            throw new InvalidOperationException($"{sectionName}:DocumentRoutePattern must start with '/' and contain '{{documentName}}'.");
        if (!option.ScalarRoute.StartsWith('/'))
            throw new InvalidOperationException($"{sectionName}:ScalarRoute must start with '/'.");
        if (option.OAuth.Enabled && (!IsHttpUrl(option.OAuth.AuthorizationUrl) || !IsHttpUrl(option.OAuth.TokenUrl)))
            throw new InvalidOperationException($"{sectionName}:OAuth requires absolute HTTP(S) AuthorizationUrl and TokenUrl values.");
        return option;
    }

    private static bool IsHttpUrl(string value)
        => Uri.TryCreate(value, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp);
}
