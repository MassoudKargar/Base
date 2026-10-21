using DotNetCore.CAP.Transport;

namespace Base.Infrastructure.Messaging.Cap;
internal class FakeTransaction : CapTransactionBase
{
    public FakeTransaction(IDispatcher dispatcher, DbContext dbContextBase) : base(dispatcher)
    {
        DbTransaction = dbContextBase;
    }

    public override void Commit()
    {
        Flush();
    }

    public override Task CommitAsync(CancellationToken cancellationToken = default)
    {
        Flush();
        return Task.CompletedTask;
    }

    protected override void AddToSent(MediumMessage msg)
    {
        base.AddToSent(msg);
    }

    public override void Dispose()
    {
        // ignore
    }

    public override void Rollback()
    {
        // ignore
    }

    public override Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        // ignore

        return Task.CompletedTask;
    }
}
