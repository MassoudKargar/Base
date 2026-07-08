namespace Base.EndPoints.Web.Extensions.Swaggers.Options;

public class SwaggerOption
{
    public bool Enabled { get; set; } = true;

    public SwaggerDocOption SwaggerDoc { get; set; } = new();

    public SwaggerOAuthOption OAuth { get; set; } = new();
}

public class SwaggerDocOption
{
    public string Title { get; set; } = "";

    public string Version { get; set; } = "v1";
}

public class SwaggerOAuthOption
{
    public bool Enabled { get; set; }

    public string AuthorizationUrl { get; set; } = "";

    public string TokenUrl { get; set; } = "";

    public Dictionary<string, string> Scopes { get; set; } = [];
}