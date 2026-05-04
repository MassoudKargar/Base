using Base.Core.Domains.Contracts.Events.Extensions;
using Base.Infrastructure.Persistence.EntityFramework.Extensions;
using Base.Infrastructure.Tools.IdGenerators; 

namespace Base.EndPoints.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSharedDefaultFrameworkServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultIntegrationEventServices();
        services.AddDataEntityFrameworkServices();
        services.AddIdGeneratorServices(configuration);
        services.AddDefaultHealthChecks(configuration);
        services.AddHttpContextAccessor();
    }
}