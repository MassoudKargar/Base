using Base.Core.Domains.Contracts.Common;
using Base.Core.Domains.Contracts.Events;
using Base.Core.Domains.Contracts.UnitOfWorks;

namespace Base.Infrastructure.Messaging.Cap;

internal class CapUnitOfWorkInterceptor : UnitOfWorkInterceptorBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IIntegrationEventBus _integrationEventBus;
    private readonly ICapPublisher _capPublisher;
    private readonly IIntegrationEventDetector _integrationEventDetector;
    private ICapTransaction? _capTransaction = null;
    private bool transactionMode = false;

    public CapUnitOfWorkInterceptor(IServiceProvider serviceProvider,
        IIntegrationEventBus integrationEventBus,
        ICapPublisher capPublisher,
        IIntegrationEventDetector integrationEventDetector)
    {
        _serviceProvider = serviceProvider;
        _integrationEventBus = integrationEventBus;
        _capPublisher = capPublisher;
        _integrationEventDetector = integrationEventDetector;
    }

    public override Task AfterBeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        transactionMode = true;
        CreateFakeTransaction();
        return base.AfterBeginTransactionAsync(cancellationToken);
    }

    public override async Task BeforeSaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await StoreEvents();
    }

    public override Task AfterSaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (!transactionMode)
        {
            PublishEvent();
        }

        return base.AfterSaveChangesAsync(cancellationToken);
    }

    public override Task AfterCommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        PublishEvent();

        return base.AfterCommitTransactionAsync(cancellationToken);
    }


    private void PublishEvent()
    {
        _capTransaction?.Commit();
    }

    private void CreateFakeTransaction()
    {
        _capTransaction ??= ActivatorUtilities.CreateInstance<FakeTransaction>(_serviceProvider);
    }

    private async Task StoreEvents()
    {
        Activity.Current?.AddEvent(new ActivityEvent("CapUnitOfWorkInterceptor start of StoreEvents"));
        CreateFakeTransaction();

        _capPublisher.Transaction = _capTransaction;

        var events = _integrationEventDetector.GetIntegrationEvents();

        foreach (var eventItem in events)
        {
            await _integrationEventBus.PublishAsync(eventItem.GetType().FullName, eventItem);
        }
        Activity.Current?.AddEvent(new ActivityEvent("CapUnitOfWorkInterceptor end of StoreEvents"));
    }
}
