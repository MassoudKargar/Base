using Base.EndPoints.Web.Middlewares;
using Base.EndPoints.Web.Swagger;

namespace Base.EndPoints.Web.Extensions;

public static class ExceptionAdapterMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionAdapter(this IApplicationBuilder app)
    {
        app.UseCustomExceptionHandler();
        return app;
    }

    public static IApplicationBuilder UseApiConsumerChecker(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiConsumerCheckerMiddleware>();
    }
}
