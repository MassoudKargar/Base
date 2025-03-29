namespace Base.Samples.Core.ApplicationServices.Products.Commands.CreateProductHandlers;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator(ITranslator translator)
    {
        RuleFor(c => c.Item).NotEmpty().WithMessage(translator[Messages.InvalidNullValue, Messages.Item]);
    }
}