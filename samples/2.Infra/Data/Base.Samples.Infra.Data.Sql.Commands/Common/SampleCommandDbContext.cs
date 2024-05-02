namespace Base.Samples.Infra.Data.Sql.Commands.Common;

public class SampleCommandDbContext(DbContextOptions<SampleCommandDbContext> options) : BaseCommandDbContext
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}