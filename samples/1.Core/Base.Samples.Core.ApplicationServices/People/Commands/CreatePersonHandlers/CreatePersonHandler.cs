namespace Base.Samples.Core.ApplicationServices.People.Commands.CreatePersonHandlers;

public class CreatePersonHandler(BaseServices baseServices, IPersonCommandRepository repository) : CommandHandler<CreatePerson, long>(baseServices)
{

    public override async Task<CommandResult<long>> Handle(CreatePerson command, CancellationToken cancellationToken)
    {
        Person person = new(new FirstName(command.FirstName), LastName.Create(command.LastName));
        await repository.InsertAsync(person, cancellationToken);
        await repository.CommitAsync(cancellationToken);
        return await OkAsync(person.Id);
    }
}