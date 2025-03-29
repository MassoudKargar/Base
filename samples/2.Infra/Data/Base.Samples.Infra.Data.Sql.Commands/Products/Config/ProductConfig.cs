namespace Base.Samples.Infra.Data.Sql.Commands.Products.Config;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(c => c.Item)
            .HasConversion(c => c.Value, c => new Item(c));
    }
}