using Microsoft.OpenApi.Interfaces;

namespace Base.EndPoints.Web.Swagger;

public class ConsumerOperationFilter : IOperationFilter
{
    public const string ConsumerKey = "x-consumer";
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var consumerNames = context.MethodInfo.GetCustomAttributes<ApiConsumerAttribute>().Select(c => c.Name).ToList();
        if (!consumerNames.Any())
        {
            return;
        }

        OpenApiArray consumerOpenApiArray = CreateConsumerOpenApiArray(operation, consumerNames);

        AddorUpdateConsumerForResponse(operation, context, consumerNames, consumerOpenApiArray);

        AddorUpdateConsumerForRequest(operation, context, consumerNames, consumerOpenApiArray);

        //  AddorUpdateConsumerForParameters(operation, consumers, consumerOpenApiArray);
    }


    private static void AddorUpdateConsumerForRequest(OpenApiOperation operation, OperationFilterContext context, List<string> consumers, OpenApiArray consumerOpenApiArray)
    {
        if (operation.RequestBody == null)
            return;

        foreach (var request in operation.RequestBody.Content)
        {
            if (context.SchemaRepository.Schemas.TryGetValue(request.Value.Schema.Reference?.Id,
                out var schema))
            {
                AddorUpdateConsumer(consumers, consumerOpenApiArray, schema.Extensions);

                foreach (var property in schema.Properties)
                {
                    if (property.Value.Reference != null)
                    {
                        if (context.SchemaRepository.Schemas.TryGetValue(property.Value.Reference.Id,
                                      out var PropertySchema))
                            AddorUpdateConsumer(consumers, consumerOpenApiArray, PropertySchema.Extensions);
                    }
                }
            }
        }
    }

    private static void AddorUpdateConsumerForResponse(OpenApiOperation operation, OperationFilterContext context, List<string> consumers, OpenApiArray consumerOpenApiArray)
    {
        if (operation.Responses == null)
            return;

        foreach (var response in operation.Responses)
        {
            foreach (var responseContent in response.Value.Content)
            {
                if (context.SchemaRepository.Schemas.TryGetValue(responseContent.Value.Schema.Reference.Id,
                    out var schema))
                {
                    AddorUpdateConsumer(consumers, consumerOpenApiArray, schema.Extensions);

                    UpdateProperties(context, consumers, consumerOpenApiArray, schema);
                }
            }
        }
    }

    private static OpenApiArray CreateConsumerOpenApiArray(OpenApiOperation operation, List<string> consumers)
    {
        var arr = new OpenApiArray();
        foreach (var consumer in consumers)
        {
            arr.Add(new OpenApiString(consumer));
        }
        operation.Extensions.Add(ConsumerKey, arr);
        return arr;
    }

    private static void UpdateProperties(OperationFilterContext context, List<string> consumers, OpenApiArray arr, OpenApiSchema schema)
    {
        if (schema.Properties == null)
            return;

        foreach (var property in schema.Properties)
        {
            if (property.Value.Reference != null)
            {
                if (context.SchemaRepository.Schemas.TryGetValue(property.Value.Reference.Id,
                              out var propertySchema))
                {
                    AddorUpdateConsumer(consumers, arr, propertySchema.Extensions);
                    UpdateProperties(context, consumers, arr, propertySchema);
                }

            }
            else if (property.Value.Items is { Reference: not null })
            {
                if (context.SchemaRepository.Schemas.TryGetValue(property.Value.Items.Reference.Id,
                             out OpenApiSchema? propertySchema))
                {
                    AddorUpdateConsumer(consumers, arr, propertySchema.Extensions);
                    UpdateProperties(context, consumers, arr, propertySchema);
                }
            }
        }
    }

    private static void AddorUpdateConsumer(List<string> consumers, OpenApiArray arr, IDictionary<string, IOpenApiExtension> extensions)
    {
        if (extensions.TryGetValue(ConsumerKey, out IOpenApiExtension? extension))
        {
            if (extension is OpenApiArray openArray)
            {
                foreach (var consumer in consumers)
                {
                    if (!openArray.Any(c => ((OpenApiString)c).Value.Equals(consumer)))
                    {
                        openArray.Add(new OpenApiString(consumer));
                    }
                }
            }
        }
        else
        {
            extensions.Add(ConsumerKey, arr);
        }
    }

    private static void AddorUpdateConsumerForParameters(OpenApiOperation operation, List<string> consumers, OpenApiArray consumerOpenApiArray)
    {
        foreach (var parameter in operation.Parameters)
        {
            if (parameter.Extensions.ContainsKey(ConsumerKey))
            {
                if (parameter.Extensions[ConsumerKey] is OpenApiArray openArray)
                {
                    foreach (var consumer in consumers)
                    {
                        if (!openArray.Any(c => c.Equals(consumer)))
                        {
                            openArray.Add(new OpenApiString(consumer));
                        }
                    }
                }
            }
            else
            {
                parameter.Extensions.Add(ConsumerKey, consumerOpenApiArray);
            }
        }
    }

}