namespace Base.Samples.Infra.Data.Sql.Commands.Common;

public class SampleCommandDbContext(DbContextOptions<SampleCommandDbContext> options) : BaseCommandDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}