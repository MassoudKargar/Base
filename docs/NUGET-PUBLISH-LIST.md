# NuGet publication list — .NET 10

Validated on 2026-10-17. Owner decision: continue the .NET 10 release using the original local branch package IDs and versions; leave public version 9 packages unchanged. No package has been pushed.

46 Base packages plus one template. Base.EndPoints.Web remains 10.0.1; every other package below remains 10.0.0. No package ID was renamed. Publish in the listed dependency order and publish the template last.

The CSV manifest contains exact paths, project paths, and SHA256 hashes. Use these artifacts, not old files in nupkgs. Earlier experimental artifacts were moved to artifacts/superseded-packages and are excluded from the release feed and manifest. Do not publish by wildcard.

| Order | Package ID | Version |
| --- | --- | --- |
| 1 | Base.Core.Domains | 10.0.0 |
| 2 | Base.Core.RequestResponse | 10.0.0 |
| 3 | Base.Extensions.BackgroundWorker.Abstractions | 10.0.0 |
| 4 | Base.Extensions.Caching.Abstractions | 10.0.0 |
| 5 | Base.Extensions.ChangeDataLog.Abstractions | 10.0.0 |
| 6 | Base.Extensions.DependencyInjection.Abstractions | 10.0.0 |
| 7 | Base.Extensions.Events.Abstractions | 10.0.0 |
| 8 | Base.Extensions.Logger.Abstractions | 10.0.0 |
| 9 | Base.Extensions.MessageBus.Abstractions | 10.0.0 |
| 10 | Base.Extensions.ObjectMappers.Abstractions | 10.0.0 |
| 11 | Base.Extensions.Serializers.Abstractions | 10.0.0 |
| 12 | Base.Extensions.Translations.Abstractions | 10.0.0 |
| 13 | Base.Extensions.UsersManagement.Abstractions | 10.0.0 |
| 14 | Base.Utility.OpenTelemetryRegistration | 10.0.0 |
| 15 | Base.Utility.Swagger.Registration | 10.0.0 |
| 16 | Base.Core.Contracts | 10.0.0 |
| 17 | Base.Extensions.BackgroundWorker.KafkaConsumer | 10.0.0 |
| 18 | Base.Extensions.BackgroundWorker.KafkaProducer | 10.0.0 |
| 19 | Base.Extensions.Caching.Distributed.Redis | 10.0.0 |
| 20 | Base.Extensions.Caching.Distributed.Sql | 10.0.0 |
| 21 | Base.Extensions.Caching.InMemory | 10.0.0 |
| 22 | Base.Extensions.ChangeDataLog | 10.0.0 |
| 23 | Base.Extensions.ChangeDataLog.Sql | 10.0.0 |
| 24 | Base.Extensions.DependencyInjection | 10.0.0 |
| 25 | Base.Extensions.Events.PollingPublisher | 10.0.0 |
| 26 | Base.Extensions.Events.PollingPublisher.Dal.Dapper | 10.0.0 |
| 27 | Base.Extensions.MessageBus.MessageInbox.Dal.Dapper | 10.0.0 |
| 28 | Base.Extensions.ObjectMappers.AutoMapper | 10.0.0 |
| 29 | Base.Extensions.Serializers.EPPlus | 10.0.0 |
| 30 | Base.Extensions.Serializers.Microsoft | 10.0.0 |
| 31 | Base.Extensions.Serializers.NewtonSoft | 10.0.0 |
| 32 | Base.Extensions.Translations.Parrot | 10.0.0 |
| 33 | Base.Extensions.UsersManagement | 10.0.0 |
| 34 | Base.Utility | 10.0.0 |
| 35 | Base.Utility.Auth.ApiAuthentication | 10.0.0 |
| 36 | Base.Utility.SerilogRegistration | 10.0.0 |
| 37 | Base.Utility.SoftwarePartsDetector | 10.0.0 |
| 38 | Base.Core.ApplicationServices | 10.0.0 |
| 39 | Base.Core.Domains.Toolkits | 10.0.0 |
| 40 | Base.Extensions.MessageBus.MessageInbox | 10.0.0 |
| 41 | Base.Extensions.MessageBus.RabbitMQ | 10.0.0 |
| 42 | Base.EndPoints.Web | 10.0.1 |
| 43 | Base.Infra.Data.Sql | 10.0.0 |
| 44 | Base.Infra.Data.Sql.Commands | 10.0.0 |
| 45 | Base.Infra.Data.Sql.Queries | 10.0.0 |
| 46 | Base.Extensions.Events.Outbox.Dal.EF | 10.0.0 |
| 47 | Base.Solution.Templates | 10.0.0 |

## Rebuild and verify

```powershell
C:\Base\scripts\Build-Packages.ps1
dotnet restore C:\Base\tests\Base.Compatibility.Smoke\Base.Compatibility.Smoke.csproj --configfile C:\Base\artifacts\nuget\NuGet.Config --packages C:\Base\artifacts\nuget\packages
dotnet run --project C:\Base\tests\Base.Compatibility.Smoke --no-restore -p:UseSharedCompilation=false
A:\Project\Collections\MassoudKargar\BaseTemplatePakage\scripts\Verify-Template.ps1 -BasePackageDirectory C:\Base\artifacts\nuget
```

The default build packs everything. -StartAt is for resuming a known partial run only and trusts skipped artifacts.

## Public package comparison

See NUGET-PUBLIC-COMPARISON.md and CSV. Public 9.x package identities remain unchanged. OpenTelemetryRegistration exists through 9.0.2 despite being absent from owner search. The existing local KafkaProducer project has no public package at its ID (HTTP 404); this would be its first publication. Legacy Base.Infrastructure, Base.Core.Domain.Toolkits, and Base.DapperService have no corresponding current production project and are not renamed or republished.

## Verification and limits

- 46 production package projects compile and pack; main solution and freshly generated template solution build.
- Native OpenAPI/Scalar HTTP smoke: 24 assertions passed on the preserved Base.EndPoints.Web 10.0.1 package.
- Legacy Swagger OAuth document generation and serialization passed before the version-only restoration; its source is unchanged.
- NU1900 prevents claiming a complete clean online vulnerability audit; existing nullable warnings remain.
- No database migration, external broker/database connection, or NuGet push was performed.
- NuGet package versions are immutable. Recheck registry state before publishing; changed content cannot overwrite an existing version.
