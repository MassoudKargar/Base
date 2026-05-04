using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.Infrastructure.Messaging.Cap.Entity.EntityTypeConfigurations;
internal class ReceivedConfiguration : IEntityTypeConfiguration<Received>
{
    private readonly string _publishedTableName;
    private readonly string? _schema;

    public ReceivedConfiguration(string publishedTableName, string? schema)
    {
        _publishedTableName = publishedTableName;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<Received> builder)
    {
        builder.HasKey(o => o.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedNever();

        builder
            .Property(p => p.Version)
            .HasMaxLength(20);

        builder
            .Property(p => p.Name)
            .HasMaxLength(200);

        builder
            .Property(p => p.Group)
            .HasMaxLength(200);

        builder
         .Property(p => p.Content)
         .IsRequired(false);

        builder
            .Property(p => p.StatusName)
            .HasMaxLength(50);

        builder.HasIndex(o => o.StatusName);
        builder.HasIndex(o => o.Name);
        builder.HasIndex(o => o.Group);
        builder.HasIndex(o => o.Added);

        builder.ToTable(_publishedTableName, _schema);
    }
}