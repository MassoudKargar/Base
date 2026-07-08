using Microsoft.OpenApi;

namespace Base.Samples.EndPoints.WebApi.Extensions.DependencyInjection.Swaggers.Filters;

public class AddParamsToHeader : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Security == null)
            operation.Security = new List<OpenApiSecurityRequirement>();

    }
}
