using Base.Core.Domain.Exceptions;
using Base.Core.Domain.ValueObjects;

namespace Base.Samples.Core.Domain.People.ValueObject;

public class LastName : BaseValueObject<LastName>
{
    public string Value { get; set; }

    public LastName(string value)
    {
        value = value.Trim();
        if (string.IsNullOrEmpty(value)) throw new InvalidValueObjectStateException(Messages.InvalidNullValue, Messages.LastName);
        if (value.Length is < 2 or > 50) throw new InvalidValueObjectStateException(Messages.InvalidStringLength, Messages.LastName, "2", "50");

        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}