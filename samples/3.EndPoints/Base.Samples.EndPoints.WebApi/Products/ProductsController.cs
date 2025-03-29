using Base.Samples.Core.Contracts.Products.Commands;

namespace Base.Samples.EndPoints.WebApi.Products;

[Route("api/[controller]")]
public class ProductsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command) => await Create<CreateProductCommand, long>(command);
}