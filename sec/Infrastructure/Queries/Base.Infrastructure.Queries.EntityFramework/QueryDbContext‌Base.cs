namespace Base.Infrastructure.Queries.EntityFramework;
public abstract class QueryDbContext‌Base : DbContext
{
    public QueryDbContext‌Base()
    {

    }

    public QueryDbContext‌Base(DbContextOptions options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       // optionsBuilder.AddInterceptors(new PersianYeKeCommandInterceptor());

        base.OnConfiguring(optionsBuilder);
    }

    #region SaveChanges
    public override int SaveChanges()
    {
        throw NotSupportException();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw NotSupportException();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        throw NotSupportException();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw NotSupportException();
    }

    private static Exception NotSupportException()
    {
        return new Exception($"{nameof(QueryDbContext‌Base)} does not support saving changes");
    }
    #endregion
}
