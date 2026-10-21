# BaseDotNet10 validation and template alignment

Validated 2026-10-17 against branch `BaseDotNet10`, baseline `b089a5f`. The active `Base.sln` references 23 projects under `sec`; all target net10.0 and retain version 10.0.0. The old Extentsions/Utility trees are not the migration source. The earlier upgrade/dotnet-10 commit is not an ancestor of this branch.

## Findings and changes

The initial Release build failed with 21 errors because Base.EndPoints.Web still used OpenAPI.NET 1.x/Swashbuckle APIs. Per owner instruction, Swashbuckle was removed from active endpoint projects. Native Microsoft.AspNetCore.OpenApi 10.0.8 generates documents; Scalar.AspNetCore 2.16.11 renders them. Microsoft.OpenApi is pinned to patched 2.7.5. Public package IDs and their 10.0.0 versions were not changed.

API consumer metadata and runtime validation remain under Base.EndPoints.Web.Consumers. Legacy Swagger generator/filter/path-list APIs were removed; existing consumers of those old APIs must migrate to native OpenAPI transformers. The default framework service registration now wires native documentation from the OpenApi configuration section. Missing/disabled configuration leaves JSON and UI routes absent. Explicit authorization metadata drives OAuth/Bearer requirements, with AllowAnonymous overrides; global fallback policies are not inferred.

The old package script traversed unrelated 8.x/9.x projects, deleted nupkgs, and did not fail reliably. The replacement scripts/Build-Packages.ps1 packages only the active solution and produces an ordered SHA256 manifest under artifacts/branch-nuget without deleting old outputs. Its isolated template feed excludes old Base package caches.

The solution template keeps its Core and src/2.Infrastructure projects. Package references now use Base.Core.Application, Base.Core.Application.Contracts, Base.Core.Domains, Base.Infrastructure.Persistence.EntityFramework, Base.Infrastructure.Queries.EntityFramework, and Base.EndPoints.Web from this branch. Old dispatcher adapters were replaced by a MediatR request pipeline; the new scoped UnitOfWork is shared across its concrete/framework/application interfaces. Command DbContext, domain-event services, mediator assembly scanning, read-only query DbContext, Persian YeKe interceptor, exception handling, native docs, and optional JWT authentication are wired to the actual branch APIs. Unused legacy adapter configuration was removed. SQL Server providers remain explicit in the application; migrations remain opt-in and were not run.

## Verification

- Initial `dotnet build Base.sln -c Release`: failed with 21 errors (artifacts/branch-dotnet10-audit.log).
- Final Release build: passed (artifacts/branch-dotnet10-fixed.log).
- Active solution pack: all 23 packages succeeded; scripts/Build-Packages.ps1 was exercised (artifacts/branch-release-script.log).
- Native OpenAPI/Scalar package-consumer HTTP smoke: 24 assertions passed (artifacts/branch-openapi-http.log).
- Template packed, installed into an isolated hive, generated as TradingSmoke, and restored/built from current branch packages with a separate package cache (template repository verify-current-base.log).
- Generated API startup/DI/HTTP smoke: 9 assertions passed (template repository verify-template-startup.log). Resolves IMediator, shared IUnitOfWork, command/query DbContexts; serves JSON/Scalar; activates the sample controller and validates the Base API result filter. Old Swagger route is absent. No database/broker connection, migration, or order was executed.
- No existing test-runner suite was found on the active branch. The added HTTP smoke is an executable dotnet run check.

## Remaining limits

This is not a clean security audit or production-readiness certification. NuGet reports known vulnerabilities in existing Magick.NET-Q16-AnyCPU 14.13.1 (Files.MiniIo module, not used by template) and transitive System.Security.Cryptography.Pkcs 9.0.4. Existing nullable/analyzer warnings remain. Some sandbox audit queries also returned NU1900; do not interpret unavailable audit data as zero vulnerabilities. External SQL/Redis/Kafka/MinIO/OAuth services were not integration-tested. CI enforcement and public NuGet publication were not performed.

## Repeat

```powershell
.\scripts\Build-Packages.ps1
dotnet restore tests/Base.OpenApi.Smoke --configfile artifacts/branch-nuget/NuGet.Config --packages artifacts/branch-nuget/packages
dotnet run --project tests/Base.OpenApi.Smoke --no-restore -p:UseSharedCompilation=false
```

From the template repository:

```powershell
.\scripts\Verify-Template.ps1 -BasePackageDirectory C:\Base\artifacts\branch-nuget
.\scripts\Smoke-GeneratedTemplate.ps1 -ProjectDirectory <generated-TradingSmoke-directory> -BasePackageDirectory C:\Base\artifacts\branch-nuget
```

## Utility .NET 10 migration

The four non-Swagger Utility libraries now target net10.0 and version 10.0.0, retaining their package IDs and public namespaces. Microsoft 9.0 references were updated to 10.0.8. SoftwarePartsDetector uses the shared Microsoft.AspNetCore.App framework instead of the legacy MVC 2.2.5 package; its unused Newtonsoft global import was removed. Existing published Base.Extensions.* 9.0.0 dependencies remain for API compatibility and are not repackaged. Sample hosts and Swagger Utility are excluded.

NuGet reports NU1902 for OpenTelemetry.Api 1.11.2 and OpenTelemetry.Exporter.OpenTelemetryProtocol 1.11.2. Upgrading this instrumentation dependency family and testing external telemetry collectors remains separate work. Package build verification does not exercise external OAuth, logging sinks or telemetry services.
