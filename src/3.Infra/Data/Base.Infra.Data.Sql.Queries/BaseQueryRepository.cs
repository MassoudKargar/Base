namespace Base.Infra.Data.Sql.Queries;
public class BaseQueryRepository<TDbContext>(TDbContext dbContext) : IQueryRepository
    where TDbContext : BaseQueryDbContext
{
    protected readonly TDbContext _dbContext = dbContext;
}
