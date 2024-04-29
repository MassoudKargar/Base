using Base.Core.Domain.ValueObjects;

namespace Base.Core.Domain.Entities;
/// <summary>
/// Base class for all entities in the system
/// </summary>
public abstract class Entity<TId> : IAuditableEntity
          where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{
    /// <summary>
    /// Entities numerical ID
    /// Be used only for saving in the database and simplicity of work.
    /// </summary>
    public TId Id { get; protected set; }
    /// <summary>
    /// Entity ID
    /// The main Entity ID that should be used everywhere is BusinessId.
    /// All communication should be established with the help of this ID.
    /// </summary>
    public BusinessId BusinessId { get; protected set; } = BusinessId.FromGuid(Guid.NewGuid());

    /// <summary>
    /// The default constructor is defined as Protected.
    /// Given that this needs to be created when constructing the basic Entity properties, no object should be created without filling these properties.
    /// To prevent this, all Entities must have constructors defined that have an input value.
    /// In order to be able to use these entities for the process of storing and retrieving from the database with the help of ORMs, it is necessary to create a default constructor with a high access level such as Protected or Private.
    /// </summary>
    protected Entity() { }


    #region Equality Check
    public bool Equals(Entity<TId>? other) => this == other;
    public override bool Equals(object? obj) =>
         obj is Entity<TId> otherObject && Id.Equals(otherObject.Id);

    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
        => !(right == left);

    #endregion
}


public abstract class Entity : Entity<long>
{

}
