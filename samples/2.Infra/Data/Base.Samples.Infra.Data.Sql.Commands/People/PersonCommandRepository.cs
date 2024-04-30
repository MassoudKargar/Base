using Base.Samples.Core.Domain.People.Entities;
using Base.Samples.Infra.Data.Sql.Commands.Common;

namespace Base.Samples.Infra.Data.Sql.Commands.People;

public class PersonCommandRepository(SampleCommandDbContext dbContext) : BaseCommandRepository<Person,SampleCommandDbContext>(dbContext)
{
}