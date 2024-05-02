namespace Base.Core.Contracts.ApplicationServices.Commands;

public interface ICommandHandler<in TCommand, TData> where TCommand : ICommand<TData>
{
    Task<CommandResult<TData>> Handle(TCommand request);
}
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<CommandResult> Handle(TCommand request);
}

