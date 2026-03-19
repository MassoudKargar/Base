namespace Base.Infra.Data.Sql.Queries.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, IFilterable queryFilter)
    {
        if (string.IsNullOrWhiteSpace(queryFilter?.Filter))
        {
            return query;
        }

        ReplaceOperators(queryFilter);

        var pagedSortableQueryFilterPreterites = typeof(PagedSortableQueryFilter<>).GetProperties();
        var queryFilterFilterProperties = typeof(IFilterable).GetProperties();
        var baseProperties = pagedSortableQueryFilterPreterites.Union(queryFilterFilterProperties);

        (string Name, object? Value)[]? props = queryFilter.GetType().GetProperties()
            .Select(p => (p.Name, p.GetValue(queryFilter))).ToArray();

        props = props.Where(p => p.Value != null && !baseProperties.Any(y => y.Name == p.Name)).ToArray();

        queryFilter.Filter = queryFilter.Filter.Replace("--", "@");
        queryFilter.Filter = queryFilter.Filter.Replace("_", " ");
        for (int i = 0; i < props.Length; i++)
        {
            (string Name, object? Value) prop = props[i];


            queryFilter.Filter = queryFilter.Filter.Replace($"@{prop.Name}", $"@{i}");

            if (prop.Value is not null && prop.Value.GetType().IsEnum)
                prop.Value = (int)prop.Value;
        }

        query = query.Where(queryFilter.Filter, props.Select(x => x.Value).ToArray());

        return query;
    }

    private static void ReplaceOperators(IFilterable queryFilter)
    {
        queryFilter.Filter = queryFilter.Filter
                    .Replace($"{Operator.Equal}", "=")
                    .Replace($"{Operator.NotEqual}", "!=")
                    .Replace($"{Operator.GreaterThan}", ">")
                    .Replace($"{Operator.GreaterThanOrEqual}", ">=")
                    .Replace($"{Operator.LessThan}", "<")
                    .Replace($"{Operator.LessThanOrEqual}", " <=");

        queryFilter.Filter = queryFilter.Filter
            .Replace($" {Operator.And} ", "&&")
            .Replace($" {Operator.Or} ", "||")
            ;

    }
}
public enum Operator
{
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    And,
    Or
}
