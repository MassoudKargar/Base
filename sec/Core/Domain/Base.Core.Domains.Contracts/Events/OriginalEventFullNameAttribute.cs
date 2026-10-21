namespace Base.Core.Domains.Contracts.Events;
public class OriginalEventFullNameAttribute : Attribute
{
    public OriginalEventFullNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
