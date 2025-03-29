namespace Base.Samples.Core.Contracts.Products.Commands;

public class CreateProductCommand(string item) : ICommand<long>
{
    public string Item { get; private set; } = item;
}