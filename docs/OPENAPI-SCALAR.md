# OpenAPI and Scalar

`Base.EndPoints.Web` 10.0.1 provides native .NET 10 OpenAPI documents and Scalar UI. New applications do not need Swashbuckle or Swagger UI.

```csharp
builder.Services.AddOpenApiDocumentation(builder.Configuration, "OpenApi");
// After builder.Build():
app.UseOpenApiDocumentation("OpenApi");
```

```json
{
  "OpenApi": {
    "Enabled": true,
    "DocumentName": "v1",
    "DocumentRoutePattern": "/openapi/{documentName}.json",
    "ScalarRoute": "/scalar",
    "Document": {
      "Title": "My API",
      "Version": "v1",
      "Description": "Application API"
    },
    "Bearer": { "Enabled": true },
    "OAuth": {
      "Enabled": false,
      "AuthorizationUrl": "https://identity.example/authorize",
      "TokenUrl": "https://identity.example/token",
      "ClientId": "public-documentation-client",
      "UsePkce": true,
      "Scopes": { "api.read": "Read API" }
    }
  }
}
```

Open `/scalar` for the UI or `/openapi/v1.json` for the document. Set `Enabled` to false to disable both; a missing configuration section also leaves them disabled. `SwaggerDoc` remains an alias for the original configuration key. `DocumentName` controls the document route independently of the API's display version.

Bearer authentication is described by default. Enable OAuth to add authorization code flow with PKCE (S256), public client ID, and configured scopes. No client secret or access token is inserted into the page. Configure the identity provider's allowed browser origins and redirect URI for the deployed Scalar page; no identity-provider calls are part of verification.

Security requirements and 401/403 responses are added to operations with explicit authorization metadata, including authorized route groups. `AllowAnonymous` overrides it. Policies applied only through a global fallback policy are not inferred by the document generator. Documentation does not enforce endpoint authorization: configure normal ASP.NET Core authentication and authorization separately.

The legacy `Base.Utility.Swagger.Registration` package remains available for existing Swashbuckle consumers and is tested separately; the solution template uses only native OpenAPI and Scalar.

## Verification

After building the framework packages:

```powershell
dotnet restore tests/Base.Compatibility.Smoke/Base.Compatibility.Smoke.csproj --configfile artifacts/nuget/NuGet.Config --packages artifacts/nuget/packages
dotnet run --project tests/Base.Compatibility.Smoke --no-restore -p:UseSharedCompilation=false
```

The smoke program starts temporary loopback HTTP servers and checks JSON/UI routes, custom document names and routes, title binding, legacy settings, Bearer/OAuth requirements, anonymous overrides, PKCE, disabled/missing settings, and invalid OAuth configuration. It does not contact a database, broker, or identity provider.
