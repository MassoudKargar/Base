namespace Base.Core.ApplicationServices.Queries;

public class QueryDispatcherDomainExceptionHandlerDecorator(
    IServiceProvider serviceProvider,
    ILogger<QueryDispatcherDomainExceptionHandlerDecorator> logger)
    : QueryDispatcherDecorator
{
    #region Fields
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<QueryDispatcherDomainExceptionHandlerDecorator> _logger = logger;
    public override int Order => 2;
    #endregion

    #region Query Dispatcher
    public override async Task<QueryResult<TData>> Execute<TQuery, TData>(TQuery query)
    {
        try
        {
            var result = await _queryDispatcher.Execute<TQuery, TData>(query);
            return result;

        }
        catch (DomainStateException ex)
        {
            _logger.LogError(BaseEventId.DomainValidationException, ex, "Processing of {QueryType} With value {Query} failed at {StartDateTime} because there are domain exceptions.", query.GetType(), query, DateTime.Now);
            return DomainExceptionHandlingWithReturnValue<TQuery, TData>(ex);
        }
        catch (AggregateException ex)
        {
            if (ex.InnerException is DomainStateException domainStateException)
            {
                _logger.LogError(BaseEventId.DomainValidationException, ex, "Processing of {QueryType} With value {Query} failed at {StartDateTime} because there are domain exceptions.", query.GetType(), query, DateTime.Now);
                return DomainExceptionHandlingWithReturnValue<TQuery, TData>(domainStateException);
            }
            throw ex;
        }
    }
    #endregion

    #region Privaite Methods
    private QueryResult<TData> DomainExceptionHandlingWithReturnValue<TQuery, TData>(DomainStateException ex)
    {
        var queryResult = new QueryResult<TData>()
        {
            Status = ApplicationServiceStatus.InvalidDomainState
        };

        queryResult.AddMessage(GetExceptionText(ex));

        return queryResult;
    }

    private string GetExceptionText(DomainStateException domainStateException)
    {
        var translator = _serviceProvider.GetService<ITranslator>();
        if (translator == null)
            return domainStateException.ToString();

        var result = domainStateException?.Parameters.Any() == true ?
             translator[domainStateException.Message, domainStateException.Parameters] :
               translator[domainStateException?.Message];

        _logger.LogInformation(BaseEventId.DomainValidationException, "Domain Exception message is {DomainExceptionMessage}", result);

        return result;
    }
    #endregion
}