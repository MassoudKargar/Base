namespace Base.Core.ApplicationServices.Queries;

public class QueryDispatcherValidationDecorator(
    IServiceProvider serviceProvider,
    ILogger<QueryDispatcherValidationDecorator> logger)
    : QueryDispatcherDecorator
{
    #region Fields
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<QueryDispatcherValidationDecorator> _logger = logger;
    public override int Order => 1;
    #endregion

    #region Query Dispatcher
    public override async Task<QueryResult<TData>> Execute<TQuery, TData>(TQuery query)
    {
        _logger.LogDebug(BaseEventId.QueryValidation, "Validating query of type {QueryType} With value {Query}  start at :{StartDateTime}", query.GetType(), query, DateTime.Now);

        var validationResult = Validate<TQuery, QueryResult<TData>>(query);

        if (validationResult != null)
        {
            _logger.LogInformation(BaseEventId.QueryValidation, "Validating query of type {QueryType} With value {Query}  failed. Validation errors are: {ValidationErrors}", query.GetType(), query, validationResult.Messages);
            return validationResult;
        }

        _logger.LogDebug(BaseEventId.QueryValidation, "Validating query of type {QueryType} With value {Query}  finished at :{EndDateTime}", query.GetType(), query, DateTime.Now);
        return await _queryDispatcher.Execute<TQuery, TData>(query);
    }
    #endregion

    #region Privaite Methods
    private TValidationResult Validate<TQuery, TValidationResult>(TQuery query) where TValidationResult : ApplicationServiceResult, new()
    {
        var validator = _serviceProvider.GetService<IValidator<TQuery>>();
        TValidationResult res = null;

        if (validator != null)
        {
            var validationResult = validator.Validate(query);
            if (!validationResult.IsValid)
            {
                res = new()
                {
                    Status = ApplicationServiceStatus.ValidationError
                };
                foreach (var item in validationResult.Errors)
                {
                    res.AddMessage(item.ErrorMessage);
                }
            }
        }
        else
        {
            _logger.LogInformation(BaseEventId.CommandValidation, "There is not any validator for {QueryType}", query.GetType());
        }
        return res;
    }
    #endregion
}