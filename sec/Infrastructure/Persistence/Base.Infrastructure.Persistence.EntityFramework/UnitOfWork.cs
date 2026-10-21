namespace Base.Infrastructure.Persistence.EntityFramework;

public class UnitOfWork(DbContext context, IEnumerable<IUnitOfWorkInterceptor> unitOfWorkInterceptors) : IUnitOfWork
{
    public void ClearChangeTracker() => context.ChangeTracker.Clear();

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return BeginTransactionAsync(cancellationToken, IsolationLevel.ReadCommitted);
    }

    public Task BeginTransactionAsync(IsolationLevel isolationLevel)
    {
        return BeginTransactionAsync(CancellationToken.None, isolationLevel);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel)
    {
        if (context.Database.CurrentTransaction == null)
        {
            await context.Database.BeginTransactionAsync
                (isolationLevel: isolationLevel, cancellationToken: cancellationToken);

            await AfterBeginTransactionAsync(cancellationToken);
        }
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Activity.Current?.AddEvent(new ActivityEvent("UnitOfWork start of SaveChangesAsync"));
        await BeforeSaveChangesAsync(cancellationToken);

        Activity.Current?.AddEvent(new ActivityEvent("UnitOfWork before SaveChangesAsync"));
        await context.SaveChangesAsync(cancellationToken);

        Activity.Current?.AddEvent(new ActivityEvent("UnitOfWork before AfterSaveChangesAsync"));
        await AfterSaveChangesAsync(cancellationToken);

        Activity.Current?.AddEvent(new ActivityEvent("UnitOfWork End of SaveChangesAsync"));
    }

    public virtual async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (context.Database.CurrentTransaction == null)
        {
            throw new InvalidOperationException("there is no external transaction");
        }

        await context.Database.CurrentTransaction.CommitAsync(cancellationToken);
        await AfterCommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (context.Database.CurrentTransaction != null)
        {
            await context.Database.CurrentTransaction.RollbackAsync(cancellationToken);
            await AfterRollbackTransactionAsync(cancellationToken);
        }
    }

    #region interceptor
    private Task BeforeSaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(unitOfWorkInterceptors.Select(c => c.BeforeSaveChangesAsync(cancellationToken)));
    }

    private Task AfterSaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(unitOfWorkInterceptors.Select(c => c.AfterSaveChangesAsync(cancellationToken)));
    }

    private Task AfterBeginTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(unitOfWorkInterceptors.Select(c => c.AfterBeginTransactionAsync(cancellationToken)));
    }


    private Task AfterCommitTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(unitOfWorkInterceptors.Select(c => c.AfterCommitTransactionAsync(cancellationToken)));
    }

    private Task AfterRollbackTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(unitOfWorkInterceptors.Select(c => c.AfterRollbackTransactionAsync(cancellationToken)));
    }
    #endregion

}
