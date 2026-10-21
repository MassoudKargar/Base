using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.Infrastructure.Messaging.Cap.Entity.EntityTypeConfigurations;
internal class PublishedConfiguration(string tableName, string? schema) : IEntityTypeConfiguration<Published>
{
    public void Configure(EntityTypeBuilder<Published> builder)
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
         .Property(p => p.Content)
         .IsRequired(false);

        builder
            .Property(p => p.StatusName)
            .HasMaxLength(50);


        builder.HasIndex(o => o.StatusName);
        builder.HasIndex(o => o.Name);
        builder.HasIndex(o => o.Added);

        builder.ToTable(tableName, schema);
    }
}