namespace Base.Samples.Infra.Data.Sql.Commands.Common;
using Base.Extensions.Events.Outbox.Dal.EF;
public class SampleCommandDbContext : BaseCommandDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
    public DbSet<Person> People { get; set; }
    public SampleCommandDbContext(DbContextOptions<SampleCommandDbContext> options) : base(options)
    {
    }
}