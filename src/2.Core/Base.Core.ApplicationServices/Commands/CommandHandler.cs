namespace Base.Core.ApplicationServices.Commands;

public abstract class CommandHandler<TCommand, TData>(BaseServices baseServices) : ICommandHandler<TCommand, TData>
    where TCommand : ICommand<TData>
{
    protected BaseServices BaseServices { get; } = baseServices;

    protected readonly CommandResult<TData> result = new();

    public abstract Task<CommandResult<TData>> Handle(TCommand command);
    protected virtual Task<CommandResult<TData>> OkAsync(TData data)
    {
        result._data = data;
        result.Status = ApplicationServiceStatus.Ok;
        return Task.FromResult(result);
    }
    protected virtual CommandResult<TData> Ok(TData data)
    {
        result._data = data;
        result.Status = ApplicationServiceStatus.Ok;
        return result;
    }
    protected virtual Task<CommandResult<TData>> ResultAsync(TData data, ApplicationServiceStatus status)
    {
        result._data = data;
        result.Status = status;
        return Task.FromResult(result);
    }

    protected virtual CommandResult<TData> Result(TData data, ApplicationServiceStatus status)
    {
        result._data = data;
        result.Status = status;
        return result;
    }

    protected void AddMessage(string message)
    {
        result.AddMessage(BaseServices.Translator[message]);
    }
    protected void AddMessage(string message, params string[] arguments)
    {
        result.AddMessage(BaseServices.Translator[message, arguments]);
    }
}

public abstract class CommandHandler<TCommand>(BaseServices baseServices) : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    protected readonly BaseServices BaseServices = baseServices;
    protected readonly CommandResult result = new();
    public abstract Task<CommandResult> Handle(TCommand command);

    protected virtual Task<CommandResult> OkAsync()
    {
        result.Status = ApplicationServiceStatus.Ok;
        return Task.FromResult(result);
    }

    protected virtual CommandResult Ok()
    {
        result.Status = ApplicationServiceStatus.Ok;
        return result;
    }

    protected virtual Task<CommandResult> ResultAsync(ApplicationServiceStatus status)
    {
        result.Status = status;
        return Task.FromResult(result);
    }
    protected virtual CommandResult Result(ApplicationServiceStatus status)
    {
        result.Status = status;
        return result;
    }
    protected void AddMessage(string message)
    {
        result.AddMessage(BaseServices.Translator[message]);
    }
    protected void AddMessage(string message, params string[] arguments)
    {
        result.AddMessage(BaseServices.Translator[message, arguments]);
    }
}

