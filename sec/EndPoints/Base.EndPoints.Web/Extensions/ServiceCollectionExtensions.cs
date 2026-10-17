using Base.EndPoints.Extensions;
using Base.EndPoints.Web.OpenApi;

namespace Base.EndPoints.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDefaultFrameworkServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedDefaultFrameworkServices(configuration);
        services.AddOpenApiDocumentation(configuration, "OpenApi");
    }
}
