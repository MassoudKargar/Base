namespace Base.EndPoints.Web.Swagger;
internal class ApiConsumerCheckerMiddleware(RequestDelegate next)
{
    private const string headerName = "x-consumer";

    public async Task InvokeAsync(HttpContext context)
    {
        string? headerConsumer = ExtractConsumerHeader(context);

        if (headerConsumer != null)
        {
            string[]? actionConsumers = ExtractActionConsumers(context);

            if (actionConsumers == null || !actionConsumers.Contains(headerConsumer))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                await context.Response.WriteAsync("invalid consumer");
                return;
            }
        }

        await next(context);
    }

    private static string[]? ExtractActionConsumers(HttpContext context)
    {
        Endpoint? endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;

        return endpoint?.Metadata.GetOrderedMetadata<ApiConsumerAttribute>()
                        .Select(c => c.Name).ToArray();
    }

    private static string? ExtractConsumerHeader(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(headerName, out StringValues value))
        {
            return value;
        }

        return null;
    }
}