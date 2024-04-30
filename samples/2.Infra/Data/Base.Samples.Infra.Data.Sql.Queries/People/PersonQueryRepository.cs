namespace Base.Samples.Infra.Data.Sql.Queries.People;

public class PersonQueryRepository(SampleQueryDbContext dbContext) : BaseQueryRepository<SampleQueryDbContext>(dbContext),IPersonQueryRepository
{
}