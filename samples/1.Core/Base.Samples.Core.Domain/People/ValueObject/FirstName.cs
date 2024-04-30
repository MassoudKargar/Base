using Base.Core.Domain.Exceptions;
using Base.Core.Domain.ValueObjects;
using Base.Utilities.Extensions;

namespace Base.Samples.Core.Domain.People.ValueObject;

public class FirstName : BaseValueObject<FirstName>
{
    public string Value { get; set; }

    public FirstName(string value)
    {
        value = value.Trim();
        if (string.IsNullOrEmpty(value)) throw new InvalidValueObjectStateException(Messages.InvalidNullValue,Messages.FirstName);
        if (value.IsLengthBetween(2,50)) throw new InvalidValueObjectStateException(Messages.InvalidStringLength,Messages.FirstName,"2","50");
        
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}