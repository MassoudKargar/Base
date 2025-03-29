namespace Base.Samples.Infra.Data.Sql.Queries.Products;

public class ProductQueryRepository(SampleQueryDbContext context) : BaseQueryRepository<SampleQueryDbContext>(context), IProductQueryRepository
{
}