using Base.Extentsions.Queries;

namespace Base.Infrastructure.Queries.EntityFramework.Extensions;
public static class PagedListExtensions
{
    public static async Task<PagedCollectionQueryResult<TSource>> ToPagedListAsync<TSource, TSortablePropertyCollection>(
        this IQueryable<TSource> source,
        PagedSortableQueryFilter<TSortablePropertyCollection> request,
        CancellationToken cancellationToken = default)
        where TSortablePropertyCollection : ISortablePropertyCollection
    {
        cancellationToken.ThrowIfCancellationRequested();

        long totalItems = await source.LongCountAsync(cancellationToken);

        if (request.Ordering != null)
        {

            source = source
            .Skip(request.SkipCount())
            .Take(request.PageSize)
            .OrderBy(request.Ordering);
        }
        else
        {
            source = source
            .Skip(request.SkipCount())
            .Take(request.PageSize);
        }

        List<TSource> items = source.ToList();

        return new PagedCollectionQueryResult<TSource>(request.PageNumber, request.PageSize, totalItems, items);
    }
}
