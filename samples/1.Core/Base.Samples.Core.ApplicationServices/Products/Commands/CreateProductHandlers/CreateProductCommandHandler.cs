using Base.Samples.Core.Domain.Products.Entities;
using Base.Samples.Core.Domain.Products.ValueObject;

namespace Base.Samples.Core.ApplicationServices.Products.Commands.CreateProductHandlers;

public class CreateProductCommandHandler(BaseServices baseServices, IProductCommandRepository commandRepository) : CommandHandler<CreateProductCommand, long>(baseServices)

{
    public override async Task<CommandResult<long>> Handle(CreateProductCommand command)
    {
        Product product = new(new Item(command.Item));
        commandRepository.Insert(product);
        await commandRepository.CommitAsync();
        return await OkAsync(product.Id);
    }
}