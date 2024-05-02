namespace Base.Core.ApplicationServices.Commands;

public class CommandDispatcher(IServiceProvider serviceProvider, ILogger<CommandDispatcher> logger)
    : ICommandDispatcher
{
    #region Fields

    private readonly Stopwatch _stopwatch = new();
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<CommandDispatcher> _logger = logger;

    #endregion

    #region Send Commands
    public async Task<CommandResult> Send<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        _stopwatch.Start();
        try
        {
            _logger.LogDebug("Routing command of type {CommandType} With value {Command}  Start at {StartDateTime}", command.GetType(), command, DateTime.Now);
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

            return await handler.Handle(command);

        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "There is not suitable handler for {CommandType} Routing failed at {StartDateTime}.", command.GetType(), DateTime.Now);
            throw;
        }
        finally
        {
            _stopwatch.Stop();
            _logger.LogInformation(BaseEventId.PerformanceMeasurement, "Processing the {CommandType} command tooks {Millisecconds} Millisecconds", command.GetType(), _stopwatch.ElapsedMilliseconds);
        }

    }

    public async Task<CommandResult<TData>> Send<TCommand, TData>(TCommand command) where TCommand : class, ICommand<TData>
    {
        _stopwatch.Start();
        try
        {
            _logger.LogDebug("Routing command of type {CommandType} With value {Command}  Start at {StartDateTime}", command.GetType(), command, DateTime.Now);
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TData>>();
            return await handler.Handle(command);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "There is not suitable handler for {CommandType} Routing failed at {StartDateTime}.", command.GetType(), DateTime.Now);
            throw;
        }
        finally
        {
            _stopwatch.Stop();
            _logger.LogInformation("Processing the {CommandType} command tooks {Millisecconds} Millisecconds", command.GetType(), _stopwatch.ElapsedMilliseconds);
        }
    }

    #endregion
}