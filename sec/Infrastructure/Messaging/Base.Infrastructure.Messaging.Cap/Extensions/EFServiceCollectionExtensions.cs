using Base.Infrastructure.Messaging.Cap.Entity.EntityTypeConfigurations;

namespace Base.Infrastructure.Messaging.Cap.Extensions;

public static class EfServiceCollectionExtensions
{
    public static void AddCapMessagingModel(this ModelBuilder modelBuilder
        , string? schema = "messaging")
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        Common.Schema = schema;

        modelBuilder.ApplyConfiguration(new PublishedConfiguration(Common.PublishedTableName, Common.Schema));
        modelBuilder.ApplyConfiguration(new ReceivedConfiguration(Common.ReceivedTableName, Common.Schema));
        // moved to EntityFrameworkStorageInitializer 
        // modelBuilder.ApplyConfiguration(new LockConfiguration(Common.LockTableName, Common.Schema));
    }
}