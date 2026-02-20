namespace Base.Infra.Data.Sql.Queries;

public abstract class PagedSortableQueryFilter<T> : IFilterable where T : ISortablePropertyCollection
{
    private int _pageNumber;
    private int _pageSize;
    private string? _ordering;
    private T _sortablePropertyCollection;

    protected PagedSortableQueryFilter(T sortablePropertyCollection)
    {
        string[] orderingSplit = sortablePropertyCollection.GetDefault().Split(" ");
        if (orderingSplit.Length == 1)
        {
            Ordering = $"{sortablePropertyCollection.GetDefault()} descending";
        }
        else
        {
            Ordering = sortablePropertyCollection.GetDefault();
        }

        _sortablePropertyCollection = sortablePropertyCollection;
        _pageNumber = 1;
        _pageSize = 10;
    }

    public string? Ordering
    {
        get
        {
            string[]? sortSplit = _ordering?.Split(" ");
            if (sortSplit?.Length == 0)
                return _ordering;

            MemberInfo? sortProperty = _sortablePropertyCollection.GetType().GetMembers().FirstOrDefault(x => x.Name.ToLower() == sortSplit[0].ToLower());
            if (sortProperty == null)
                return _ordering;

            return $"{(string)((FieldInfo)sortProperty).GetValue(_sortablePropertyCollection)} {sortSplit[1] ?? ""}";
        }
        set
        {
            _ordering = value;
        }
    }

    public int PageNumber
    {
        get
        {
            return _pageNumber;
        }
        set
        {
            _pageNumber = value > 1 ? value : 1;
        }
    }
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = value >= 1 ? value : 10;
        }
    }

    public string? Filter { get; set; }

    public int SkipCount() => (PageNumber * PageSize) - PageSize;

}