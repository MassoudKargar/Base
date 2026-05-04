using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.Infrastructure.Messaging.Cap.Entity.EntityTypeConfigurations;
internal class LockConfiguration : IEntityTypeConfiguration<CapLock>
{
    private readonly string _tableName;
    private readonly string? _schema;

    public LockConfiguration(string tableName, string? schema)
    {
        _tableName = tableName;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<CapLock> builder)
    {
        builder.HasKey(o => o.Key);

        builder
            .Property(p => p.Key)
            .HasMaxLength(128)
            .ValueGeneratedNever();

        builder
            .Property(p => p.Instance)
            .HasMaxLength(256);

        builder
            .Property(p => p.LastLockTime)
            .IsRequired();

        builder.ToTable(_tableName, _schema);
    }
}