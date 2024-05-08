using Base.Samples.Core.Contracts.People.Commands;

namespace Base.Samples.Infra.Data.Sql.Commands.People;

public class PersonCommandRepository(SampleCommandDbContext dbContext) : BaseCommandRepository<Person, SampleCommandDbContext>(dbContext), IPersonCommandRepository
{
}