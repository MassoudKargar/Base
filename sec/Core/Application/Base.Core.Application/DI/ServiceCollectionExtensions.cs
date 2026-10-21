using Base.Core.Application.Clocks;
using Base.Core.Application.CustomeMediatR;
using Base.Core.Domains.Contracts.Common;

namespace Base.Core.Application.DI;

public static class ServiceCollectionExtensions
{
    public static void AddMediatRServices(this IServiceCollection services, params Assembly[] assembles)
    {
        services.AddMediatR(options =>
        {
            options.MediatorImplementationType = typeof(CustomMediator);
            options.RegisterServicesFromAssemblies(assembles);
        });
    }
    public static void AddClock(this IServiceCollection services)
    {
        services.AddScoped<IClock, SystemClock>();
    }
}