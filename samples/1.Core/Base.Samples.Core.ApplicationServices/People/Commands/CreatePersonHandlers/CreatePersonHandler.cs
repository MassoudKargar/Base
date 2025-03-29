namespace Base.Samples.Core.ApplicationServices.People.Commands.CreatePersonHandlers;

public class CreatePersonHandler(BaseServices baseServices, IPersonCommandRepository repository) : CommandHandler<CreatePerson, long>(baseServices)
{

    public override async Task<CommandResult<long>> Handle(CreatePerson command)
    {
        Person person = new(new FirstName(command.FirstName), new LastName(command.LastName));
        await repository.InsertAsync(person);
        await repository.CommitAsync();
        return await OkAsync(person.Id);
    }
}