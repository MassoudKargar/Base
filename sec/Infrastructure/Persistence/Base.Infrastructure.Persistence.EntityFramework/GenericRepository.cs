namespace Base.Infrastructure.Persistence.EntityFramework;

public abstract class GenericRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : struct
{
    /// <summary>
    /// سازنده کلاس که با پایگاه داده مرتبط است.
    /// </summary>
    /// <param name="dbContext">کانتکست پایگاه داده</param>
    protected GenericRepository(DbContext dbContext)
    {
        Context = dbContext;
        DbSet = Context.Set<TEntity>();
    }

    protected DbContext Context { get; }
    protected DbSet<TEntity> DbSet { get; }

    /// <summary>
    /// درج موجودیت به صورت ناهمزمان در پایگاه داده.
    /// </summary>
    /// <param name="entity">موجودیت برای درج</param>
    /// <param name="cancellationToken">توکن لغو عملیات</param>
    public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var result = await DbSet.AddAsync(entity, cancellationToken);
        entity.CreationDate = DateTime.UtcNow;
        entity.Deleted = false;
        return result.Entity;
    }

    /// <summary>
    /// موجودیت موجود را به‌روزرسانی می‌کند.
    /// </summary>
    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Context.Entry(entity).State = EntityState.Modified;
        entity.ModificationDate = DateTime.UtcNow;
    }

    /// <summary>
    /// بروزرسانی وضعیت حذف شده (IsDeleted) برای موجودیت.
    /// </summary>
    /// <param name="entity">موجودیت برای بروزرسانی</param>
    /// <param name="cancellationToken"></param>
    public virtual async Task UpdateDeletedAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Context.Entry(entity).State = EntityState.Modified;
        entity.ModificationDate = DateTime.UtcNow;
        entity.Deleted = true;
    }

    /// <summary>
    /// دریافت تمام موجودیت‌ها به همراه ویژگی‌های اضافی (شامل).
    /// </summary>
    /// <param name="predicate"> شرط هایی که باید شامل شود</param>
    /// <param name="cancellationToken"></param>
    /// <returns>یک لیست که تمام موجودیت‌ها را به همراه ویژگی‌های اضافی باز می‌گرداند</returns>
    public virtual async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) =>
        await GetQuery().Where(predicate).ToListAsync(cancellationToken);

    /// <summary>
    /// دریافت تمام موجودیت‌ها از پایگاه داده به صورت ناهمزمان.
    /// </summary>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken) =>
        await GetQuery().ToListAsync(cancellationToken);

    /// <summary>
    /// بر اساس شرط ورودی تعداد را محاصبه میکند
    /// </summary>
    /// <param name="predicate">شروط روردی</param>
    /// <returns></returns>
    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) =>
        DbSet.CountAsync(predicate, cancellationToken);

    /// <summary>
    /// دریافت موجودیتی با شناسه خاص.
    /// </summary>
    /// <param name="id">شناسه موجودیت</param>
    /// <param name="cancellationToken"></param>
    /// <returns>موجودیتی با شناسه خاص</returns>
    public virtual Task<TEntity> GetSingleAsync(TId id, CancellationToken cancellationToken) =>
        GetQuery().SingleAsync(EntityHelper.CreateEqualityExpressionForId<TEntity, TId>(id), cancellationToken);

    /// <summary>
    /// دریافت موجودیتی با شناسه خاص.
    /// </summary>
    /// <param name="id">شناسه موجودیت</param>
    /// <param name="cancellationToken"></param>
    /// <returns>موجودیتی با شناسه خاص</returns>
    public virtual Task<TEntity?> GetFindAsync(TId id, CancellationToken cancellationToken) =>
        GetQuery().FirstOrDefaultAsync(EntityHelper.CreateEqualityExpressionForId<TEntity, TId>(id), cancellationToken: cancellationToken);

    /// <summary>
    /// دریافت موجودیتی با شناسه خاص.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>موجودیتی با شناسه خاص</returns>
    public virtual Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) =>
        GetQuery().FirstOrDefaultAsync(predicate, cancellationToken);

    /// <summary>
    /// بررسی وجود موجودیت بر اساس شناسه.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns>آیا موجودیت با شناسه خاص وجود دارد؟</returns>
    public virtual Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) =>
        GetQuery().AnyAsync(predicate, cancellationToken);

    protected virtual IQueryable<TEntity> GetQuery()
    {
        var query = DbSet.AsQueryable();
        var includes = GetIncludes();
        if (!includes.Any()) return query;
        foreach (var include in includes)
        {
            if (include.Body.NodeType == ExpressionType.Constant)
            {
                if (include.Body is ConstantExpression memberExpression)
                {
                    query = query.Include(memberExpression.ToString());
                }
            }
            else
            {
                query = query.Include(include);
            }
        }

        return query;
    }
    public Task SaveChangeAsync(CancellationToken cancellationToken) =>
        Context.SaveChangesAsync(cancellationToken);

    protected abstract IList<Expression<Func<TEntity, object?>>> GetIncludes();

}