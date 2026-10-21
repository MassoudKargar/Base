namespace Base.Infrastructure.Queries.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryDbContext<TContext>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder>? optionsAction = null)
        where TContext : QueryDbContext‌Base
    {
        serviceCollection.AddDbContext<TContext>(optionsAction);

        return serviceCollection;
    }

    public static DbContextOptionsBuilder AddPersianYeKeCommandInterceptor(this DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder;
    }
}
