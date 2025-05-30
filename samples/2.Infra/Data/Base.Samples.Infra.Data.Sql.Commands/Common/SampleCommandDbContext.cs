using System.Reflection;

namespace Base.Samples.Infra.Data.Sql.Commands.Common;
using Base.Extensions.Events.Outbox.Dal.EF;
public class SampleCommandDbContext : BaseCommandDbContext
{
    protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();
    public DbSet<Person> People { get; set; }
    public DbSet<Product> Products { get; set; }
    public SampleCommandDbContext(DbContextOptions<SampleCommandDbContext> options) : base(options)
    {
    }
}