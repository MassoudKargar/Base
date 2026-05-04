namespace Base.EndPoints.Web.Swagger;


[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class ApiConsumerAttribute : Attribute
{
    public string Name { get; }

    public ApiConsumerAttribute(string name)
    {
        Name = name;
    }
}