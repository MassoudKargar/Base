namespace Base.Samples.Core.Contracts.People.Commands;

public interface IPersonCommandRepository : ICommandRepository<Person, long>
{
}