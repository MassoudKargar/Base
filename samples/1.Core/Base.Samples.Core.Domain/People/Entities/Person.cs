namespace Base.Samples.Core.Domain.People.Entities;

public class Person : AggregateRoot
{
    #region Properties
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    #endregion

    public Person(FirstName firstName, LastName lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        AddEvent(new PersonCreated(BusinessId.Value, firstName.Value, lastName.Value));
    }
}