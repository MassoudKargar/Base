﻿namespace Base.Infra.Data.Sql.Commands.Extensions;
public static class ChangeTrackerExtensions
{
    public static List<AggregateRoot> GetChangedAggregates(this ChangeTracker changeTracker) =>
        changeTracker.Aggregates().Where(IsModified()).Select(c => c.Entity).ToList();

    public static List<AggregateRoot> GetAggregatesWithEvent(this ChangeTracker changeTracker) =>
            changeTracker.Aggregates()
                .Where(IsNotDetached()).Select(c => c.Entity).Where(c => c.GetEvents().Any()).ToList();
    public static IEnumerable<EntityEntry<AggregateRoot>> Aggregates(this ChangeTracker changeTracker) =>
        changeTracker.Entries<AggregateRoot>();

    private static Func<EntityEntry<AggregateRoot>, bool> IsNotDetached() =>
        x => x.State != EntityState.Detached;

    private static Func<EntityEntry<AggregateRoot>, bool> IsModified()
    {
        return x => x.State is EntityState.Modified or EntityState.Added or EntityState.Deleted;
    }

}
