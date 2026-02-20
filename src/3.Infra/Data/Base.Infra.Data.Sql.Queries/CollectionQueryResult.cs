namespace Base.Infra.Data.Sql.Queries;

public class CollectionQueryResult<T>(IEnumerable<T> items, int totalItems)
{
    public CollectionQueryResult(IEnumerable<T> items) : this(items, items.Count())
    {
    }

    public long TotalItems { get; set; } = totalItems;
    public IEnumerable<T> Items { get; set; } = items;
}