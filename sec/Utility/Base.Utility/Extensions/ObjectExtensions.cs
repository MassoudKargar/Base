namespace Base.Utility.Extensions;
/// <summary>
/// کلاس افزونه برای تبدیل یک شیء به رشته Query String.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// شیء ورودی را به فرمت Query String تبدیل می‌کند.
    /// </summary>
    /// <param name="obj">شیء ورودی که باید به Query String تبدیل شود.</param>
    /// <returns>یک رشته که نمایانگر Query String ساخته شده است.</returns>
    /// <exception cref="ArgumentNullException">اگر شیء ورودی نال باشد، خطا پرتاب می‌کند.</exception>
    public static string ToQueryString(this object obj)
    {
        if (obj is null) throw new ArgumentNullException("Object");

        // دریافت خصوصیات شیء که مقدار آن‌ها قابل نوشتن و غیر نال است
        var properties = obj.GetType().GetProperties()
            .Where(x => x.CanWrite)
            .Where(x => x.GetValue(obj, null) is not null)
            .Select(x => KeyValuePair.Create(x.Name, x.GetValue(obj, null))).ToList();

        // شناسایی خصوصیات که از نوع IEnumerable (به جز رشته‌ها) هستند
        var propertyNames = properties
            .Where(x => x.Value is not string && x.Value is IEnumerable)
            .Select(x => x.Key)
            .ToList();

        // پردازش خصوصیات IEnumerable
        foreach (var key in propertyNames)
        {
            var valueType = properties.FirstOrDefault(x => x.Key == key).Value.GetType();

            var valueElemType = valueType.IsGenericType
                ? valueType.GetGenericArguments()[0]
                : valueType.GetElementType();

            if (valueElemType.IsPrimitive || valueElemType == typeof(string) || valueElemType == typeof(Guid))
            {
                var enumerable = properties.FirstOrDefault(c => c.Key == key).Value as IEnumerable;

                properties.RemoveAll(x => x.Key == key);

                foreach (var item in enumerable)
                {
                    properties.Add(KeyValuePair.Create(key, item));
                }
            }
        }

        // ساختن رشته Query String
        return string.Join("&", properties.Where(x => x.Value is not null)
            .Select(x => string.Concat(
                Uri.EscapeDataString(x.Key), "=",
                Uri.EscapeDataString(x.Value.ToString()))));
    }
}
