# .NET 10 validation and template release

Date: 2026-10-17

## Baseline

- Framework: `C:\Base`, branch `upgrade/dotnet-10`, baseline commit `81e3c6b683fce08d34a04d602b62c2eb63124c5d`.
- Template: `A:\Project\Collections\MassoudKargar\BaseTemplatePakage`, branch `upgrade-dotnet-10`, baseline commit `dcc20a4a880db22b924463d3540eb98c3bdce336`.
- Both worktrees had existing owner changes. No commit, checkout, reset, or publication was performed.
- SDK 10.0.401 was used; 10.0.113 is also installed. `dotnet --info` had a workload/Service Control Manager permission error; compilation itself works.
- Production framework projects already targeted net10.0. The template packaging project still targeted net9.0.

## Changes

- Kept the existing layered architecture and framework/template identities.
- Repaired PollingPublisher to use the existing `ISendMessageBus.Send(Parcel)` contract and property-based Parcel, retaining event ID, correlation ID, route, payload, and headers.
- Aligned production RabbitMQ and EPPlus dependencies from Base 9.0.0 to 10.0.0.
- Fixed the legacy Swagger utility for the installed Swashbuckle 10/OpenAPI.NET 2 API and document-aware security requirements; its package version remains the original 10.0.0.
- Pinned Microsoft.OpenApi 2.7.5 in the two consuming framework packages to address GHSA-v5pm-xwqc-g5wc. Base.EndPoints.Web is now 10.0.1.
- Completed native OpenAPI/Scalar integration, settings compatibility, document-aware Bearer/OAuth security references, PKCE, document names/routes, and explicit authorization/anonymous metadata handling. See OPENAPI-SCALAR.md.
- Replaced destructive recursive batch packing with ordered package builds, an isolated internal package cache, per-package logs, and artifact/hash manifests. Old nupkgs are preserved.
- Upgraded template packaging to net10.0, suppressed assembly dependency groups for the content-only template, excluded IDE/build output, and retained CLI launch settings.
- Preserved existing mixed-case tracked template paths. Packaging merges legacy Templates content into the canonical templates package path on case-sensitive hosts; Linux execution was not available for verification.
- Generated applications use Base.EndPoints.Web 10.0.1 and native OpenAPI/Scalar without Swashbuckle. Corrected opt-in migration context to the command DbContext, connected configured authentication middleware, and removed unconditional identity-server TLS certificate acceptance.

## Validation

- All 46 production framework package projects compiled and packed for net10.0. Their archives contain net10.0 assemblies.
- The main Base solution (including its existing samples) restored from the fresh local feed and built in Release.
- Legacy Swagger OAuth2 document generation and serialization passed a local smoke check.
- The tracked OpenAPI/Scalar smoke program passed 24 assertions against temporary loopback HTTP servers, using the actual Base.EndPoints.Web nupkg.
- The template nupkg was inspected, installed into an isolated absolute-path template hive, and used to generate TradingSmoke. The generated solution contains renamed projects and CLI launch settings, and restores/builds in Release against the local Base feed.
- Commands/logs: `pack-dotnet10.log`, `artifacts/nuget/logs`, `artifacts/base-solution-final.log`, `artifacts/openapi-smoke/http-verify.log`, and the template repository's `verify-dotnet10.log`.
- Some sandboxed network requests failed; loopback HTTP checks succeeded with approved execution outside that network restriction.

## Limits and remaining risks

- Existing nullable/compiler warnings remain; no broad refactor was attempted.
- NuGet audit endpoint requests returned NU1900. This report does not claim a complete clean vulnerability/deprecation audit.
- Database migrations and external SQL/Redis/RabbitMQ/Kafka/OAuth connectivity were not executed. No trading integration was added.
- Global fallback authorization policies are not inferred by API documentation; explicit authorization metadata is documented.
- There was no existing automated test suite. A repeatable package-consumer smoke program was added under tests/Base.Compatibility.Smoke; it runs with dotnet run, not a test-framework runner.
- Public package IDs/versions were compared with NuGet search and selected direct version indices. The owner approved retaining original local .NET 10 versions and leaving 9.x unchanged. Never overwrite a published package version with changed content.
- The combined release manifest records 47 artifacts: 46 framework packages plus Base.Solution.Templates. Publish the template last.

Final version preservation: Base.EndPoints.Web 10.0.1, all other framework/template packages 10.0.0. The two temporary extra version increments were reverted and the final artifacts rebuilt. Final native HTTP checks passed 24 assertions on Base.EndPoints.Web 10.0.1.
