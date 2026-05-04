using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Base.EndPoints.ConfigurationViewers;

public static class ConfigurationViewerWebApplicationExtensions
{
    public static WebApplication UseConfigurationViewer(this WebApplication app, string path = "/configviewer",
        params object[] endpointMetadatas)
    {

        var routeHandlerBuilder = app.MapGet(path, async (HttpContext httpContext,
            [FromServices] IConfiguration configuration,
            [FromQuery] ConfigurationViewerType? type
            ) =>
        {
            //if (!await Authentication(httpContext))
            //{
            //    if (httpContext.Response.StatusCode != StatusCodes.Status302Found)
            //        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    return "401";
            //}

            var root = configuration as IConfigurationRoot;
            return root.GetConfigView(type);
        });


        routeHandlerBuilder.Add((endpointBuilder) =>
        {
            foreach (var endpointMetadata in endpointMetadatas)
            {
                endpointBuilder.Metadata.Add(endpointMetadata);
            }
        });

        return app;
    }

    //internal static async Task<bool> Authentication(HttpContext context)
    //{
    //    if (!context.User?.Identity?.IsAuthenticated ?? false)
    //    {
    //        var result = await context.AuthenticateAsync(SystemBasicAuthenticationHandler.AuthenticationScheme);

    //        if (!result.Succeeded)
    //        {
    //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //            return false;
    //        }
    //    }

    //    return true;
    //}

}