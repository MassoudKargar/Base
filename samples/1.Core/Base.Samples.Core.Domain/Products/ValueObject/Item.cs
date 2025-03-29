namespace Base.Samples.Core.Domain.Products.ValueObject;

public class Item : BaseValueObject<Item>
{
    public string Value { get; set; }

    public Item(string value)
    {
        value = value.Trim();
        if (string.IsNullOrEmpty(value)) throw new InvalidValueObjectStateException(Messages.InvalidNullValue,Messages.Item);
        if (!value.IsLengthBetween(2,50)) throw new InvalidValueObjectStateException(Messages.InvalidStringLength,Messages.Item, "2","50");
        
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}