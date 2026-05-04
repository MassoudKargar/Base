namespace Base.EndPoints.Web.Swagger;

public static class SwaggerPathsBuilderExtensions
{
    /// <summary>
    /// defaul path = swagger/{documentName}/paths.json
    /// </summary>
    /// <param name="app"></param>
    public static void UseSwaggerPaths(this IEndpointRouteBuilder app, string? basePath = null)
    {
        app.UseSwaggerPaths("swagger/{documentName}/paths.json", basePath);
    }

    public static void UseSwaggerPaths(this IEndpointRouteBuilder app, string path, string? basePath = null)
    {
        app.MapGet(path,
        async (HttpContext context, string documentName) => await SetPathsToResponse(context, documentName, basePath));
    }

    public static bool FilterToApiConsumer(string docname, ApiDescription apiDescription)
    {
        if (!apiDescription.TryGetMethodInfo(out MethodInfo methodInfo))
            return false;

        var docnameSplites = docname.Split('-');
        if (docnameSplites.Length != 2)
            return true;

        var consumers = methodInfo.GetCustomAttributes(true)
                    .OfType<ApiConsumerAttribute>()
                    .Select(attr => attr.Name)
                    .ToArray();

        if (consumers == null)
            return false;

        return consumers.Any(c => c.Equals(docnameSplites[1]));
    }

    private static async Task SetPathsToResponse(HttpContext context, string documentName, string? basePath)
    {
        var swaggerProvider = context.RequestServices.GetRequiredService<IAsyncSwaggerProvider>();
        var openApiDocument = await swaggerProvider.GetSwaggerAsync(documentName, null, basePath);
        var list = new List<object>();
        var server = openApiDocument.Servers.FirstOrDefault()?.Url;
        foreach (var path in openApiDocument.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                var Verb = operation.Key as OperationType?;
                var OprationPath = path.Key;
                if (server is not null)
                {
                    OprationPath = UrlCombine(server, OprationPath);
                }
                list.Add(new { Path = OprationPath, Type = Verb?.ToString("G") });
            }
        }

        await context.Response.WriteAsJsonAsync(list);
    }

    public static string UrlCombine(string url1, string url2)
    {
        if (url1.Length == 0)
        {
            return url2;
        }

        if (url2.Length == 0)
        {
            return url1;
        }

        url1 = url1.TrimEnd('/', '\\');
        url2 = url2.TrimStart('/', '\\');

        return string.Format("{0}/{1}", url1, url2);
    }
   
}