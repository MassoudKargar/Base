namespace Base.Core.Domains.Contracts.UnitOfWorks;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(IsolationLevel isolationLevel);

    Task BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    void ClearChangeTracker();
}
