namespace Base.Samples.Core.Domain.Products.Events;

public record ProductCreated(Guid BusinessId, string Item) : IDomainEvent;