using Base.Samples.Core.Domain.Products.ValueObject;

namespace Base.Samples.Core.Domain.Products.Entities;

public class Product : AggregateRoot
{
    #region Properties
    public Item Item { get; private set; }
    #endregion

    public Product(Item item)
    {
        Item = item;
    }
}