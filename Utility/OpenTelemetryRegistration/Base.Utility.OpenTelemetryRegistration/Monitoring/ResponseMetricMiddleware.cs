namespace Base.Utilities.OpenTelemetryRegistration.Monitoring;
public class ResponseMetricMiddleware(RequestDelegate request)
{
    private readonly RequestDelegate _request = request ?? throw new ArgumentNullException(nameof(request));

    public async Task Invoke(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value ?? "/";
        var options = httpContext.RequestServices.GetRequiredService<IOptions<OpenTelemetryOptions>>().Value;
        var serviceName = $"{options.ApplicationName}.{options.ServiceName}";
        MetricReporter reporter = new MetricReporter(serviceName, "http");

        if (path == "/metrics")
        {
            await _request.Invoke(httpContext);
            return;
        }
        var sw = Stopwatch.StartNew();
        try
        {
            using (new Activity(path).Start())
            {
                await _request.Invoke(httpContext);
            }
        }
        finally
        {
            sw.Stop();
            reporter.RegisterRequest(path);
            reporter.RegisterResponseTime(httpContext.Response.StatusCode,
                httpContext.Request.Method, path, sw.Elapsed);
        }
    }

}