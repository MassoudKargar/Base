namespace Base.Samples.Core.Contracts.People.Commands;

public class CreatePerson : ICommand<long>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}