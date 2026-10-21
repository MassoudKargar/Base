using Base.Core.Domains.Contracts.UnitOfWorks;
using DotNetCore.CAP.Filter;

namespace Base.Infrastructure.Messaging.Cap;
internal class CapSubscribeFilter : SubscribeFilter
{
    protected const string UnhandledExceptionMessage = "An unhandled exception has been occurred.";
    protected const string UnhandledEventExceptionMessage = "An unhandled integrtionEventException has been occurred.";

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CapSubscribeFilter> _logger;
    public CapSubscribeFilter(IServiceProvider serviceProvider, ILogger<CapSubscribeFilter> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override async Task OnSubscribeExceptionAsync(ExceptionContext context)
    {
        await RollBackTransaction(); ;

        _logger.LogError(context.Exception, UnhandledEventExceptionMessage, context.DeliverMessage);

        await base.OnSubscribeExceptionAsync(context);
    }


    private async Task RollBackTransaction()
    {
        try
        {
            var unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
            if (unitOfWork != null)
                await unitOfWork.RollbackTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, UnhandledExceptionMessage);
        }
    }

    public override Task OnSubscribeExecutingAsync(ExecutingContext context)
    {
        return base.OnSubscribeExecutingAsync(context);
    }
}