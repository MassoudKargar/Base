namespace Base.Infra.Data.Sql.Commands;

public class BaseCommandRepository<TEntity, TDbContext, TId>(TDbContext dbContext)
    : ICommandRepository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TDbContext : BaseCommandDbContext
    where TId : struct,
    IComparable,
    IComparable<TId>,
    IConvertible,
    IEquatable<TId>,
    IFormattable
{

    protected readonly TDbContext DbContext = dbContext;


    public void Delete(TId id)
    {
        var entity = DbContext.Set<TEntity>().Find(id);
        if (entity != null)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }
    }

    public void Delete(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    public void DeleteGraph(TId id)
    {
        var entity = GetGraph(id);
        if (entity is not null && !entity.Id.Equals(default))
            DbContext.Set<TEntity>().Remove(entity);
    }

    #region insert

    public void Insert(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }
    #endregion

    #region Get Single Item
    public TEntity? Get(TId id)
    {
        return DbContext.Set<TEntity>().Find(id);
    }

    public TEntity? Get(BusinessId businessId)
    {
        return DbContext.Set<TEntity>().FirstOrDefault(c => c.BusinessId == businessId);
    }

    public async Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken)
    {
        return await DbContext.Set<TEntity>().FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> GetAsync(BusinessId businessId, CancellationToken cancellationToken)
    {
        return await DbContext.Set<TEntity>().FirstOrDefaultAsync(c => c.BusinessId == businessId, cancellationToken);
    }
    #endregion

    #region Get single item with graph
    public TEntity? GetGraph(TId id)
    {
        var graphPath = DbContext.GetIncludePaths(typeof(TEntity)).ToArray();
        var query = DbContext.Set<TEntity>().AsQueryable();
        query = graphPath.Aggregate(query, (current, item) => current.Include(item));
        return query.FirstOrDefault(c => c.Id.Equals(id));
    }

    public TEntity? GetGraph(BusinessId businessId)
    {
        var graphPath = DbContext.GetIncludePaths(typeof(TEntity)).ToArray();
        var query = DbContext.Set<TEntity>().AsQueryable();
        query = graphPath.Aggregate(query, (current, item) => current.Include(item));
        return query.FirstOrDefault(c => c.BusinessId == businessId);
    }

    public async Task<TEntity?> GetGraphAsync(TId id, CancellationToken cancellationToken)
    {
        var graphPath = DbContext.GetIncludePaths(typeof(TEntity));
        var query = DbContext.Set<TEntity>().AsQueryable();
        query = graphPath.Aggregate(query, (current, item) => current.Include(item));
        return await query.FirstOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
    }

    public async Task<TEntity?> GetGraphAsync(BusinessId businessId, CancellationToken cancellationToken)
    {
        var graphPath = DbContext.GetIncludePaths(typeof(TEntity));
        var query = DbContext.Set<TEntity>().AsQueryable();
        query = graphPath.Aggregate(query, (current, item) => current.Include(item));
        return await query.FirstOrDefaultAsync(c => c.BusinessId == businessId, cancellationToken);
    }
    #endregion

    #region Exists
    public bool Exists(Expression<Func<TEntity, bool>> expression)
    {
        return DbContext.Set<TEntity>().Any(expression);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return await DbContext.Set<TEntity>().AnyAsync(expression, cancellationToken);
    }
    #endregion

    #region Transaction management
    public int Commit()
    {
        return DbContext.SaveChanges();
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
    public void BeginTransaction()
    {
        DbContext.BeginTransaction();
    }

    public void CommitTransaction()
    {
        DbContext.CommitTransaction();
    }
    public void RollbackTransaction()
    {
        DbContext.RollbackTransaction();
    }
    #endregion
}


public class BaseCommandRepository<TEntity, TDbContext>(TDbContext dbContext)
    : BaseCommandRepository<TEntity, TDbContext, long>(dbContext)
    where TEntity : AggregateRoot
    where TDbContext : BaseCommandDbContext;