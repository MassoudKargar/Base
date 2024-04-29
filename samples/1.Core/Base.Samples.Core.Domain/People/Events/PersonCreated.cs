using Base.Core.Domain.Events;

namespace Base.Samples.Core.Domain.People.Events;

public record PersonCreated(Guid BusinessId, string FirstName, string LastName) : IDomainEvent;