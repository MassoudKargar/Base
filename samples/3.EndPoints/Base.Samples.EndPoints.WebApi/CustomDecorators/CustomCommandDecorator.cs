﻿using Base.Core.RequestResponse.Commands;

namespace Base.Samples.EndPoints.WebApi.CustomDecorators;

public class CustomCommandDecorator : CommandDispatcherDecorator
{
    public override int Order => 0;

    public override async Task<CommandResult> Send<TCommand>(TCommand command)
    {
        return await _commandDispatcher.Send(command);
    }

    public override async Task<CommandResult<TData>> Send<TCommand, TData>(TCommand command)
    {
        return await _commandDispatcher.Send<TCommand, TData>(command);
    }
}