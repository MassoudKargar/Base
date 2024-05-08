namespace Base.Samples.Infra.Data.Sql.Commands.Common;

public class SampleUnitOfWork(SampleCommandDbContext dbContext) : BaseEntityFrameworkUnitOfWork<SampleCommandDbContext>(dbContext), ISampleUnitOfWork
{

}