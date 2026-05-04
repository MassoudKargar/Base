namespace Base.Core.Domains.Contracts.UnitOfWorks;

public interface IUnitOfWorkInterceptor
{
    Task AfterBeginTransactionAsync(CancellationToken cancellationToken = default);

    Task BeforeSaveChangesAsync(CancellationToken cancellationToken = default);

    Task AfterSaveChangesAsync(CancellationToken cancellationToken = default);

    Task AfterCommitTransactionAsync(CancellationToken cancellationToken = default);

    Task AfterRollbackTransactionAsync(CancellationToken cancellationToken = default);
}
