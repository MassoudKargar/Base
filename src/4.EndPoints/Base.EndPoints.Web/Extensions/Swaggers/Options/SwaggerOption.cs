namespace Base.EndPoints.Web.Extensions.Swaggers.Options;

public class SwaggerOption
{
    public bool Enabled { get; set; } = true;
    public string DocumentName { get; set; } = "v1";
    public string DocumentRoutePattern { get; set; } = "/openapi/{documentName}.json";
    public string ScalarRoute { get; set; } = "/scalar";
    public SwaggerDocOption Document { get; set; } = new();
    // Keep the original configuration key working for existing applications.
    public SwaggerDocOption SwaggerDoc { get => Document; set => Document = value; }
    public SwaggerOAuthOption OAuth { get; set; } = new();
    public SwaggerBearerOption Bearer { get; set; } = new();
}

public class SwaggerDocOption
{
    public string Title { get; set; } = "API";
    public string Version { get; set; } = "v1";
    public string Description { get; set; } = "";
}

public class SwaggerOAuthOption
{
    public bool Enabled { get; set; }
    public string AuthorizationUrl { get; set; } = "";
    public string TokenUrl { get; set; } = "";
    public string ClientId { get; set; } = "";
    public bool UsePkce { get; set; } = true;
    public Dictionary<string, string> Scopes { get; set; } = [];
}

public class SwaggerBearerOption
{
    public bool Enabled { get; set; } = true;
    public string Description { get; set; } = "Enter a JWT access token.";
}
