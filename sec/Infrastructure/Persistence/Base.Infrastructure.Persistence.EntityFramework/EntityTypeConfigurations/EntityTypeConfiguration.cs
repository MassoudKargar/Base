namespace Base.Infrastructure.Persistence.EntityFramework.EntityTypeConfigurations;

public abstract class EntityTypeConfiguration<TEntity, TId>(string schema) : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TId>
    where TId : struct
{

    internal string Schema { get; } = schema;

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.ToTable(typeof(TEntity).Name, Schema);
    }
}