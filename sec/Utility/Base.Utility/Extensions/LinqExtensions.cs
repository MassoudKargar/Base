using Base.Utility.Extensions;

namespace Base.Utility.Extensions;

public static class LinqExtensions
{
    /// <summary>
    /// مرتب‌سازی نتایج استعلام بر اساس یک فیلد مشخص.
    /// </summary>
    /// <typeparam name="T">نوع اлемент‌های استعلام</typeparam>
    /// <param name="q">استعلام جمع‌آوری شده</param>
    /// <param name="SortField">فیلد براساس که مرتب‌سازی می‌شود</param>
    /// <param name="Ascending">اگر برابر با true، مرتب‌سازی صعودی خواهد بود؛ در غیر این صورت نزولی</param>
    /// <returns>استعلام مرتب شده</returns>
    public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, bool Ascending)
    {
        var param = Expression.Parameter(typeof(T), "p");
        var prop = Expression.Property(param, SortField);
        var exp = Expression.Lambda(prop, param);
        string method = Ascending ? "OrderBy" : "OrderByDescending";
        Type[] types = new Type[] { q.ElementType, exp.Body.Type };
        var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
        return q.Provider.CreateQuery<T>(mce);
    }

    /// <summary>
    /// یک لیست از обiect‌ها را به یک DataTable تبدیل می‌کند.
    /// </summary>
    /// <typeparam name="T">نوع آیتم‌های لیست</typeparam>
    /// <param name="list">لیست آیتم‌های</param>
    /// <returns>DataTable حاوی داده‌های از لیست</returns>
    public static DataTable ToDataTable<T>(this List<T> list)
    {
        DataTable dataTable = new(typeof(T).Name);

        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo prop in Props)
        {
            dataTable.Columns.Add(prop.Name);
        }

        var count = Props.Length;
        foreach (T item in list)
        {
            var values = new object?[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }
        return dataTable;
    }

    /// <summary>
    /// یک DataTable را به یک لیست از обiect‌ها تبدیل می‌کند.
    /// </summary>
    /// <typeparam name="T">نوع آیتم‌های لیست</typeparam>
    /// <param name="dt">DataTable</param>
    /// <returns>لیست حاوی داده‌های از DataTable</returns>
    public static List<T> ToList<T>(this DataTable dt)
    {
        return (from DataRow row in dt.Rows select row.GetItem<T>()).ToList();
    }

    /// <summary>
    /// یک سطر داده (DataRow) را به یک ابجکت از نوع T تبدیل می‌کند.
    /// </summary>
    /// <typeparam name="T">نوع ابجکت</typeparam>
    /// <param name="dr">DataRow</param>
    /// <returns>ابجکت از نوع T حاوی داده‌های از سطر داده</returns>
    public static T GetItem<T>(this DataRow dr)
    {
        Type genericType = typeof(T);
        T obj = Activator.CreateInstance<T>();
        foreach (DataColumn column in dr.Table.Columns)
        {
            PropertyInfo? property = genericType.GetProperties().FirstOrDefault(p => p.Name == column.ColumnName);
            if (property is not null)
                property.SetValue(obj, Convert.ChangeType(dr[column], property.PropertyType), null);
        }
        return obj;
    }


    /// <summary>
    /// اگر شرط داده شده صحیح باشد، یک IQueryable<T> را با استفاده از پیش‌فرضیت مورد نظر فیلتر می‌کند.
    /// </summary>
    /// <param name="query">IQueryable<T> که بر روی آن فیلتر اعمال می‌شود</param>
    /// <param name="condition">مقدار بولینی</param>
    /// <param name="predicate">پیش‌فرضیت برای فیلتر کردن</param>
    /// <returns>IQueryable<T> فیلتر شده یا نفی از حالت فیلتر شده با توجه به مقدار شرط</returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition
            ? query.Where(predicate)
            : query;
    }


    /// <summary>
    /// اگر شرط داده شده صحیح باشد، یک IQueryable<T> را با استفاده از پیش‌فرضیت مورد نظر فیلتر می‌کند.
    /// </summary>
    /// <param name="query">IQueryable<T> که بر روی آن فیلتر اعمال می‌شود</param>
    /// <param name="condition">مقدار بولینی</param>
    /// <param name="predicate">پیش‌فرضیت با پارامتر دوم به شکل (item, index) برای فیلتر کردن</param>
    /// <returns>IQueryable<T> فیلتر شده یا نفی از حالت فیلتر شده با توجه به مقدار شرط</returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
    {
        return condition
            ? query.Where(predicate)
            : query;
    }

}
