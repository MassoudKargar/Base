namespace Base.EndPoints.Web.OpenApi;

public class ApiDocumentationOptions
{
    public bool Enabled { get; set; } = true;
    public string DocumentName { get; set; } = "v1";
    public string DocumentRoutePattern { get; set; } = "/openapi/{documentName}.json";
    public string ScalarRoute { get; set; } = "/scalar";
    public ApiDocumentOptions Document { get; set; } = new();
    public ApiOAuthOptions OAuth { get; set; } = new();
    public ApiBearerOptions Bearer { get; set; } = new();
}

public class ApiDocumentOptions
{
    public string Title { get; set; } = "API";
    public string Version { get; set; } = "v1";
    public string Description { get; set; } = "";
}

public class ApiOAuthOptions
{
    public bool Enabled { get; set; }
    public string AuthorizationUrl { get; set; } = "";
    public string TokenUrl { get; set; } = "";
    public string ClientId { get; set; } = "";
    public bool UsePkce { get; set; } = true;
    public Dictionary<string, string> Scopes { get; set; } = [];
}

public class ApiBearerOptions
{
    public bool Enabled { get; set; } = true;
    public string Description { get; set; } = "Enter a JWT access token.";
}
