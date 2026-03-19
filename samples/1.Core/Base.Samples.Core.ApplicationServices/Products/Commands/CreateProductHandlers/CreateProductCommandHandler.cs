using Base.Samples.Core.Domain.Products.Entities;
using Base.Samples.Core.Domain.Products.ValueObject;

namespace Base.Samples.Core.ApplicationServices.Products.Commands.CreateProductHandlers;

public class CreateProductCommandHandler(BaseServices baseServices, IProductCommandRepository commandRepository) : CommandHandler<CreateProductCommand, long>(baseServices)

{
    public override async Task<CommandResult<long>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = new(new Item(command.Item));
        await commandRepository.InsertAsync(product, cancellationToken);
        await commandRepository.CommitAsync(cancellationToken);
        return await OkAsync(product.Id);
    }
}