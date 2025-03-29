namespace Base.Samples.Core.Domain;

public abstract class Messages
{
    public static readonly string InvalidNullValue = "The value of {0} should not be null";
    public static readonly string InvalidNumberValueRange = "The value of {0} should not be less than";
    public static readonly string InvalidStringLength = "The length of {0} should be {1} - {2}";
    public static readonly string FirstName = nameof(FirstName);
    public static readonly string LastName = nameof(LastName);
    public static readonly string Item = nameof(Item);
}