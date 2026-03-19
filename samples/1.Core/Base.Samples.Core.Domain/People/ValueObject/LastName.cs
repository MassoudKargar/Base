namespace Base.Samples.Core.Domain.People.ValueObject;

public class LastName : BaseValueObject<LastName>
{
    public string Value { get; set; }

    private LastName(string value)
    {
        value = value.Trim();
        if (string.IsNullOrEmpty(value)) throw new InvalidValueObjectStateException(Messages.InvalidNullValue, Messages.LastName);
        if (value.Length is < 2 or > 50) throw new InvalidValueObjectStateException(Messages.InvalidStringLength, Messages.LastName, "2", "50");

        Value = value;
    }

    public static LastName Create(string value)
    {
        return new LastName(value);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}