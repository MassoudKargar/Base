namespace Base.Samples.Core.ApplicationServices.People.Commands.CreatePersonHandlers;

public class CreatePersonHandler(BaseServices baseServices, IPersonCommandRepository repository) : CommandHandler<CreatePerson, long>(baseServices)
{
    private IPersonCommandRepository Repository { get; } = repository;

    public override async Task<CommandResult<long>> Handle(CreatePerson command)
    {
        Person person = new(new FirstName(command.FirstName), new LastName(command.LastName));
        await Repository.InsertAsync(person);
        await Repository.CommitAsync();
        return await OkAsync(person.Id);
    }
}