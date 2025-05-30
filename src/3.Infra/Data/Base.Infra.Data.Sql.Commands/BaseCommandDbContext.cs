using System.Reflection;

namespace Base.Infra.Data.Sql.Commands;
public abstract class BaseCommandDbContext : DbContext
{
    protected IDbContextTransaction _transaction;
    protected abstract Assembly ConfigurationsAssembly { get; }

    public BaseCommandDbContext(DbContextOptions options) : base(options)
    {

    }

    public void BeginTransaction()
    {
        _transaction = Database.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }
        _transaction.Rollback();
    }

    public void CommitTransaction()
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }
        HandleSoftDelete();
        AddTimestamps();
        _transaction.Commit();
    }

    public T GetShadowPropertyValue<T>(object entity, string propertyName) where T : IConvertible
    {
        var value = Entry(entity).Property(propertyName).CurrentValue;
        return value != null
            ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture)
            : default;
    }

    public object GetShadowPropertyValue(object entity, string propertyName)
    {
        return Entry(entity).Property(propertyName).CurrentValue;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.AddAuditableShadowProperties();
        builder.ApplyConfigurationsFromAssembly(ConfigurationsAssembly);
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            // بررسی ارث‌بری از BaseEntity
            if (entityType.ClrType.BaseType != null &&
                entityType.ClrType.BaseType.IsGenericType &&
                entityType.ClrType.BaseType.GetGenericTypeDefinition() == typeof(Entity<>))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var filter = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, "Deleted"),
                        Expression.Constant(false)
                    ),
                    parameter
                );
                entityType.SetQueryFilter(filter);
            }
        }
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<Description>().HaveConversion<DescriptionConversion>();
        configurationBuilder.Properties<Title>().HaveConversion<TitleConversion>();
        configurationBuilder.Properties<BusinessId>().HaveConversion<BusinessIdConversion>();
        configurationBuilder.Properties<LegalNationalId>().HaveConversion<LegalNationalId>();
        configurationBuilder.Properties<NationalCode>().HaveConversion<NationalCodeConversion>();

    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleSoftDelete();
        AddTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }
    public override int SaveChanges()
    {
        HandleSoftDelete();
        AddTimestamps();
        return base.SaveChanges();
    }
    private void HandleSoftDelete()
    {
        foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
        {
            var entityType = entry.Entity.GetType();
            var baseEntityType = entityType.BaseType;

            while (baseEntityType != null)
            {
                if (baseEntityType.IsGenericType && baseEntityType.GetGenericTypeDefinition() == typeof(Entity<>))
                {
                    var deletedProperty = entityType.GetProperty(nameof(Entity<int>.Deleted));

                    if (deletedProperty != null && deletedProperty.PropertyType == typeof(bool))
                    {
                        entry.State = EntityState.Modified;
                        deletedProperty.SetValue(entry.Entity, true);
                    }
                    break;
                }
                baseEntityType = baseEntityType.BaseType;
            }
        }
    }
    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entity in entities)
        {
            var entityType = entity.Entity.GetType();
            var baseEntityType = entityType.BaseType;

            while (baseEntityType != null)
            {
                if (baseEntityType.IsGenericType && baseEntityType.GetGenericTypeDefinition() == typeof(Entity<>))
                {
                    var modifiedProperty = entityType.GetProperty(nameof(Entity<int>.ModificationDate));

                    if (modifiedProperty != null && modifiedProperty.PropertyType == typeof(DateTime?))
                    {
                        modifiedProperty.SetValue(entity.Entity, DateTime.UtcNow);
                    }
                    break;
                }
                baseEntityType = baseEntityType.BaseType;
            }
        }
    }
    public IEnumerable<string> GetIncludePaths(Type clrEntityType)
    {
        var entityType = Model.FindEntityType(clrEntityType);
        var includedNavigations = new HashSet<INavigation>();
        var stack = new Stack<IEnumerator<INavigation>>();
        while (true)
        {
            var entityNavigations = new List<INavigation>();
            foreach (var navigation in entityType.GetNavigations())
            {
                if (includedNavigations.Add(navigation))
                    entityNavigations.Add(navigation);
            }
            if (entityNavigations.Count == 0)
            {
                if (stack.Count > 0)
                    yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
            }
            else
            {
                foreach (var navigation in entityNavigations)
                {
                    var inverseNavigation = navigation.Inverse;
                    if (inverseNavigation != null)
                        includedNavigations.Add(inverseNavigation);
                }
                stack.Push(entityNavigations.GetEnumerator());
            }
            while (stack.Count > 0 && !stack.Peek().MoveNext())
                stack.Pop();
            if (stack.Count == 0) break;
            entityType = stack.Peek().Current.TargetEntityType;
        }
    }
}