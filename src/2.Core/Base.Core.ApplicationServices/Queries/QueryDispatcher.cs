namespace Base.Core.ApplicationServices.Queries;

public class QueryDispatcher(IServiceProvider serviceProvider, ILogger<QueryDispatcher> logger)
    : IQueryDispatcher
{
    #region Fields
    private readonly Stopwatch _stopwatch = new();
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<QueryDispatcher> _logger = logger;
    #endregion


    #region Query Dispatcher
    public Task<QueryResult<TData>> Execute<TQuery, TData>(TQuery query) where TQuery : class, IQuery<TData>
    {
        _stopwatch.Start();
        try
        {
            _logger.LogDebug("Routing query of type {QueryType} With value {Query}  Start at {StartDateTime}", query.GetType(), query, DateTime.Now);
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TData>>();
            return handler.Handle(query);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "There is not suitable handler for {QueryType} Routing failed at {StartDateTime}.", query.GetType(), DateTime.Now);
            throw;
        }
        finally
        {
            _stopwatch.Stop();
            _logger.LogInformation(BaseEventId.PerformanceMeasurement, "Processing the {QueryType} query tooks {Millisecconds} Millisecconds", query.GetType(), _stopwatch.ElapsedMilliseconds);
        }
    }
    #endregion
}