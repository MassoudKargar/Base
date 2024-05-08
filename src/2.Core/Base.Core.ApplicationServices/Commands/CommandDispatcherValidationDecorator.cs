namespace Base.Core.ApplicationServices.Commands;

public class CommandDispatcherValidationDecorator(
    IServiceProvider serviceProvider,
    ILogger<CommandDispatcherValidationDecorator> logger)
    : CommandDispatcherDecorator
{
    #region Fields
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<CommandDispatcherValidationDecorator> _logger = logger;
    public override int Order => 1;
    #endregion

    #region Send Commands
    public override async Task<CommandResult> Send<TCommand>(TCommand command)
    {
        _logger.LogDebug(BaseEventId.CommandValidation, "Validating command of type {CommandType} With value {Command}  start at :{StartDateTime}", command.GetType(), command, DateTime.Now);
        var validationResult = Validate<TCommand, CommandResult>(command);

        if (validationResult != null)
        {
            _logger.LogInformation(BaseEventId.CommandValidation, "Validating command of type {CommandType} With value {Command}  failed. Validation errors are: {ValidationErrors}", command.GetType(), command, validationResult.Messages);
            return validationResult;
        }
        _logger.LogDebug(BaseEventId.CommandValidation, "Validating command of type {CommandType} With value {Command}  finished at :{EndDateTime}", command.GetType(), command, DateTime.Now);
        return await _commandDispatcher.Send(command);
    }

    public override async Task<CommandResult<TData>> Send<TCommand, TData>(TCommand command)
    {
        _logger.LogDebug(BaseEventId.CommandValidation, "Validating command of type {CommandType} With value {Command}  start at :{StartDateTime}", command.GetType(), command, DateTime.Now);

        var validationResult = Validate<TCommand, CommandResult<TData>>(command);

        if (validationResult != null)
        {
            _logger.LogInformation(BaseEventId.CommandValidation, "Validating command of type {CommandType} With value {Command}  failed. Validation errors are: {ValidationErrors}", command.GetType(), command, validationResult.Messages);
            return validationResult;
        }
        _logger.LogDebug(BaseEventId.CommandValidation, "Validating command of type {CommandType} With value {Command}  finished at :{EndDateTime}", command.GetType(), command, DateTime.Now);
        return await _commandDispatcher.Send<TCommand, TData>(command);
    }
    #endregion

    #region Privaite Methods
    private TValidationResult Validate<TCommand, TValidationResult>(TCommand command) where TValidationResult : ApplicationServiceResult, new()
    {
        var validator = _serviceProvider.GetService<IValidator<TCommand>>();
        TValidationResult res = null;

        if (validator != null)
        {
            var validationResult = validator.Validate(command);
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
            _logger.LogInformation(BaseEventId.CommandValidation, "There is not any validator for {CommandType}", command.GetType());
        }
        return res;
    }
    #endregion
}
