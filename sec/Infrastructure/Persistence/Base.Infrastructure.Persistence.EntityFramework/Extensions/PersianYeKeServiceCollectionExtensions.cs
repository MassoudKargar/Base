namespace Base.Infrastructure.Persistence.EntityFramework.Extensions;

public static class PersianYeKeServiceCollectionExtensions
{
    public static DbContextOptionsBuilder AddPersianYeKeCommandInterceptor(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new PersianYeKeCommandInterceptor());

        return optionsBuilder;
    }
}