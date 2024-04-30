namespace Base.Samples.Core.ApplicationServices.People.Commands.CreatePersonHandlers;

public class CreatePersonValidator : AbstractValidator<CreatePerson>
{
    public CreatePersonValidator(ITranslator translator)
    {
        RuleFor(c => c.FirstName).NotEmpty().WithMessage(translator[Messages.InvalidNullValue,Messages.FirstName]);
        RuleFor(c => c.LastName).NotEmpty().WithMessage(translator[Messages.InvalidNullValue,Messages.LastName]);
    }
}