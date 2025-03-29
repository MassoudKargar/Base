using Base.Samples.Core.Contracts.Products.Commands;

namespace Base.Samples.Infra.Data.Sql.Commands.Products;

public class ProductCommandRepository(SampleCommandDbContext context) : BaseCommandRepository<Product, SampleCommandDbContext>(context), IProductCommandRepository
{
}