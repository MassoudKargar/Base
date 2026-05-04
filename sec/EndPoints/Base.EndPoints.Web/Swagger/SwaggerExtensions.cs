namespace Base.EndPoints.Web.Swagger;
public static class SwaggerExtensions
{
    public static void FilterToConsumer(this OpenApiDocument swaggerDoc, string consumer)
    {
        foreach (var path in swaggerDoc.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                var isQCommerce = operation.Value.Extensions.Any(
                                  c => c.Key.Equals(ConsumerOperationFilter.ConsumerKey, StringComparison.InvariantCulture)
                                  &&
                                  ((OpenApiArray)c.Value).Any(c => ((OpenApiString)c).Value == consumer));
                if (!isQCommerce)
                {
                    path.Value.Operations.Remove(operation);
                }
            }

            if (path.Value.Operations.Count == 0)
            {
                swaggerDoc.Paths.Remove(path.Key);
            }
        }


        foreach (var schema in swaggerDoc.Components.Schemas.ToList())
        {
            var isQCommerce = schema.Value.Extensions.Any(
                                c => c.Key.Equals(ConsumerOperationFilter.ConsumerKey, StringComparison.InvariantCulture)
                                &&
                                ((OpenApiArray)c.Value).Any(c => ((OpenApiString)c).Value == consumer));

            if (!isQCommerce)
                swaggerDoc.Components.Schemas.Remove(schema);
        }
    }
}
