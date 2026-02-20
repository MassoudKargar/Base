namespace Base.Infra.Data.Sql.Queries;

public class PagedCollectionQueryResult<T>(int pageNumber, int pageSize, long totalItems, IEnumerable<T> items)
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public long TotalItems { get; } = totalItems;
    public IEnumerable<T> Items { get; } = items;
}