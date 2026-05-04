namespace Base.EndPoints.Web.Swagger;

public class ClientVersionHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        var schema = new OpenApiSchema();
        schema.Type = "string";

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "client-name",
            In = ParameterLocation.Header,
            Required = false,
            Schema = schema,
            Example = new OpenApiString("Swagger on " + Assembly.GetEntryAssembly()?.GetName().Name)
        });

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "client-version",
            In = ParameterLocation.Header,
            Schema = schema,
            Required = false,
            Example = new OpenApiString(Assembly.GetEntryAssembly().GetName().Version.ToString())
        });
    }
}