namespace Base.Infra.Data.Sql.Commands.Interceptors;

public class AddChangeDataLogInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SaveEntityChangeLogs(eventData);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SaveEntityChangeLogs(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private  void SaveEntityChangeLogs(DbContextEventData eventData)
    {
        var changeTracker = eventData.Context.ChangeTracker;
        var userInfoService = eventData.Context.GetService<IUserInfoService>();
        var itemRepository = eventData.Context.GetService<IEntityChangeInterceptorItemRepository>();
        var options = eventData.Context.GetService<IOptions<ChangeDataLogHamsterOptions>>().Value;
        var changedEntities = GetChangedEntities(changeTracker);
        var transactionId = Guid.NewGuid().ToString();
        var dateOfAccured = DateTime.Now;

        var entityChangeInterceptorItems = new List<EntityChangeInterceptorItem>();

        foreach (var entity in changedEntities)
        {
            var entityChangeInterceptorItem = new EntityChangeInterceptorItem
            {
                Id = Guid.NewGuid(),
                TransactionId = transactionId,
                UserId = userInfoService.UserId().ToString(),
                Ip = userInfoService.GetUserIp(),
                EntityType = entity.Entity.GetType().FullName,
                EntityId = entity.Property(options.BusinessIdFieldName).CurrentValue.ToString(),
                DateOfOccurrence = dateOfAccured,
                ChangeType = entity.State.ToString(),
                ContextName = GetType().FullName
            };

            foreach (var property in entity.Properties.Where(c => options.propertyForReject.All(d => d != c.Metadata.Name)))
            {
                if (entity.State == EntityState.Added || property.IsModified)
                {
                    entityChangeInterceptorItem.PropertyChangeLogItems.Add(new PropertyChangeLogItem
                    {
                        ChangeInterceptorItemId = entityChangeInterceptorItem.Id,
                        PropertyName = property.Metadata.Name,
                        Value = property.CurrentValue?.ToString(),
                    });
                }
            }
            entityChangeInterceptorItems.Add(entityChangeInterceptorItem);
        }
        itemRepository.Save(entityChangeInterceptorItems);


    }
 
    private  List<EntityEntry> GetChangedEntities(ChangeTracker changeTracker) =>
       changeTracker.Entries()
           .Where(x => x.State == EntityState.Modified
           || x.State == EntityState.Added
           || x.State == EntityState.Deleted).ToList();
}
