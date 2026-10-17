# Public NuGet comparison

Source: https://azuresearch-usnc.nuget.org/query?q=MasoudKargar&prerelease=true&take=100

Checked 2026-10-17: 48 search results. Direct NuGet version indices were also checked for endpoint, Swagger, template, OpenTelemetry, and KafkaProducer.

Owner decision: continue with the original local .NET 10 versions and leave 9.x packages alone. All package IDs and original local package versions are preserved. The two extra version increments introduced during this session were reverted before final packing.

| Package ID | Public latest | Initial local | Final artifact | Identity |
| --- | --- | --- | --- | --- |
| Base.Core.Domains | 9.0.1 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Core.RequestResponse | 9.0.3 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.BackgroundWorker.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Caching.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.ChangeDataLog.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.DependencyInjection.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Events.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Logger.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.MessageBus.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.ObjectMappers.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Serializers.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Translations.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.UsersManagement.Abstractions | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Utility.OpenTelemetryRegistration | 9.0.2 (direct index; absent from search) | 10.0.0 | 10.0.0 | Exact existing ID; search omitted it |
| Base.Utility.Swagger.Registration | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Core.Contracts | 9.0.4 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.BackgroundWorker.KafkaConsumer | 8.0.1 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.BackgroundWorker.KafkaProducer | 404 in direct index | 10.0.0 | 10.0.0 | No public package found for this local ID |
| Base.Extensions.Caching.Distributed.Redis | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Caching.Distributed.Sql | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Caching.InMemory | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.ChangeDataLog | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.ChangeDataLog.Sql | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.DependencyInjection | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Events.PollingPublisher | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Events.PollingPublisher.Dal.Dapper | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.MessageBus.MessageInbox.Dal.Dapper | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.ObjectMappers.AutoMapper | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Serializers.EPPlus | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Serializers.Microsoft | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Serializers.NewtonSoft | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Translations.Parrot | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.UsersManagement | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Utility | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Utility.Auth.ApiAuthentication | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Utility.SerilogRegistration | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Utility.SoftwarePartsDetector | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Core.ApplicationServices | 9.0.4 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Core.Domains.Toolkits | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.MessageBus.MessageInbox | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.MessageBus.RabbitMQ | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.EndPoints.Web | 9.0.3 | 10.0.1 | 10.0.1 | Exact existing ID |
| Base.Infra.Data.Sql | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Infra.Data.Sql.Commands | 9.0.4 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Infra.Data.Sql.Queries | 9.0.4 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Extensions.Events.Outbox.Dal.EF | 9.0.0 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Solution.Templates | 9.0.4 | 10.0.0 | 10.0.0 | Exact existing ID |
| Base.Infrastructure | 8.3.2 | NO LOCAL PROJECT | NOT BUILT | Published legacy ID; preserve |
| Base.Core.Domain.Toolkits | 8.0.0 | NO LOCAL PROJECT | NOT BUILT | Published legacy ID; preserve |
| Base.DapperService | 2.5.0 | NO LOCAL PROJECT | NOT BUILT | Published legacy ID; preserve |

Search is not a complete historical inventory. OpenTelemetryRegistration exists through 9.0.2 but is absent from the supplied search. KafkaProducer returned HTTP 404. No public package identity or version was changed.
