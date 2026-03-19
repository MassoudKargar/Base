using System.Linq.Expressions;

namespace Base.Core.ApplicationServices.Queries;

public static class SortExtensions
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, Dictionary<string, Expression<Func<T, object>>> sortFunctions, string propertyName, SortDirection direction)
        where T : class
    {
        KeyValuePair<string, Expression<Func<T, object>>> sortFunc = sortFunctions.FirstOrDefault(k => k.Key.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        query = direction == SortDirection.Asc ? query.OrderBy(sortFunc.Value) : query.OrderByDescending(sortFunc.Value);
        return query;
    }
}