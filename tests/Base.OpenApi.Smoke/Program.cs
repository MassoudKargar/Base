using System.Text.Json.Nodes;
using Base.EndPoints.Web.OpenApi;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Logging;

var checks = 0;
await Verify(new() {
    ["OpenApi:Enabled"] = "true", ["OpenApi:Document:Title"] = "Scalar Smoke",
    ["OpenApi:Document:Version"] = "2026", ["OpenApi:DocumentName"] = "v2",
    ["OpenApi:DocumentRoutePattern"] = "/specs/{documentName}.json", ["OpenApi:ScalarRoute"] = "/docs",
    ["OpenApi:OAuth:Enabled"] = "true", ["OpenApi:OAuth:AuthorizationUrl"] = "https://example.invalid/authorize",
    ["OpenApi:OAuth:TokenUrl"] = "https://example.invalid/token", ["OpenApi:OAuth:ClientId"] = "smoke-client",
    ["OpenApi:OAuth:Scopes:read"] = "Read access"
}, "/specs/v2.json", "/docs", "Scalar Smoke", true);
await Verify(new() { ["OpenApi:Enabled"] = "true", ["OpenApi:Document:Title"] = "Configured document" },
    "/openapi/v1.json", "/scalar", "Configured document", false);
await Verify(new() { ["OpenApi:Enabled"] = "false" }, "/openapi/v1.json", "/scalar", null, false);
await Verify(new(), "/openapi/v1.json", "/scalar", null, false);
var invalidBuilder = WebApplication.CreateBuilder();
invalidBuilder.Configuration.AddInMemoryCollection(new Dictionary<string,string?> {
    ["OpenApi:Enabled"] = "true", ["OpenApi:OAuth:Enabled"] = "true"
});
try { invalidBuilder.Services.AddOpenApiDocumentation(invalidBuilder.Configuration, "OpenApi"); throw new Exception("Invalid OAuth configuration accepted"); }
catch (InvalidOperationException ex) when (ex.Message.Contains("AuthorizationUrl")) { checks++; }
Console.WriteLine($"OpenAPI/Scalar HTTP smoke passed: {checks} assertions.");

async Task Verify(Dictionary<string,string?> settings, string jsonPath, string uiPath, string? title, bool oauth)
{
    var builder = WebApplication.CreateBuilder();
    builder.Logging.ClearProviders();
    builder.WebHost.UseUrls("http://127.0.0.1:0");
    builder.Configuration.AddInMemoryCollection(settings);
    builder.Services.AddAuthorization();
    builder.Services.AddOpenApiDocumentation(builder.Configuration, "OpenApi");
    await using var app = builder.Build();
    app.UseOpenApiDocumentation("OpenApi");
    app.MapGet("/public", () => "public").AllowAnonymous();
    app.MapGet("/secure", () => "secure").RequireAuthorization();
    app.MapGroup("/group").RequireAuthorization().MapGet("/anonymous", () => "public").AllowAnonymous();
    await app.StartAsync();
    try {
        var address = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!.Addresses.Single();
        using var client = new HttpClient { BaseAddress = new Uri(address) };
        var response = await client.GetAsync(jsonPath);
        if (title is null) {
            Require(response.StatusCode == System.Net.HttpStatusCode.NotFound, "Disabled document must be absent");
            Require((await client.GetAsync(uiPath)).StatusCode == System.Net.HttpStatusCode.NotFound, "Disabled UI must be absent");
            return;
        }
        response.EnsureSuccessStatusCode();
        var document = JsonNode.Parse(await response.Content.ReadAsStringAsync())!;
        Require(document["info"]!["title"]!.GetValue<string>() == title, "Configured title");
        var secure = document["paths"]!["/secure"]!["get"]!;
        Require(secure["security"]!.AsArray().Any(x => x!["Bearer"] is not null), "Bearer requirement");
        Require(secure["responses"]!["401"] is not null && secure["responses"]!["403"] is not null, "Authorization responses");
        Require(document["paths"]!["/public"]!["get"]!["security"] is null, "Public endpoint security");
        Require(document["paths"]!["/group/anonymous"]!["get"]!["security"] is null, "AllowAnonymous overrides group authorization");
        Require(document["components"]!["securitySchemes"]!["Bearer"]!["scheme"]!.GetValue<string>() == "bearer", "Bearer schema");
        var htmlResponse = await client.GetAsync(uiPath);
        htmlResponse.EnsureSuccessStatusCode();
        var html = await htmlResponse.Content.ReadAsStringAsync();
        Require(html.Contains(title) && html.Contains("scalar", StringComparison.OrdinalIgnoreCase), "Scalar UI and title");
        Require(!html.Contains("Medication Schedule"), "No unrelated hardcoded title");
        if (oauth) {
            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "scalar-smoke.html"), html);
            Require(document["components"]!["securitySchemes"]!["OAuth2"]!["flows"]!["authorizationCode"]!["scopes"]!["read"] is not null, "OAuth scheme and scopes");
            Require(secure["security"]!.AsArray().Any(x => x!["OAuth2"] is not null), "OAuth requirement");
            Require(html.Contains("smoke-client") && html.Contains("\"x-usePkce\":\"SHA-256\""), "Scalar OAuth client and PKCE");
        }
    } finally { await app.StopAsync(); }
}
void Require(bool condition, string message) { if (!condition) throw new Exception(message); checks++; }
