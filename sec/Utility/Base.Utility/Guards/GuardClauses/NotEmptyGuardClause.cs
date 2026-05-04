using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;
public static class NotEmptyGuardClause
{
    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی خالی نیست و در صورت خالی بودن، استثنا پرتاب می‌شود.
    /// این متد برای انواع مختلف مانند رشته‌ها، مجموعه‌ها و انواع عمومی استفاده می‌شود.
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید بررسی شود</param>
    /// <param name="message">پیام خطا که در صورت خالی بودن مقدار ورودی، برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت خالی بودن مقدار ورودی</exception>
    public static void NotEmpty<T>(this Guard guard, T value, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value == null)
            throw new InvalidOperationException(message);

        if (value is ICollection collectionValue && collectionValue.Count == 0)
            throw new InvalidOperationException(message);

        if (value is IEnumerable enumerableValue && !enumerableValue.GetEnumerator().MoveNext())
            throw new InvalidOperationException(message);

        if (value is string strValue && string.IsNullOrWhiteSpace(strValue))
            throw new InvalidOperationException(message);

        if (EqualityComparer<T>.Default.Equals(value, default))
            throw new InvalidOperationException(message);
    }

    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی با استفاده از مقایسگر ارائه شده خالی نیست و در صورت خالی بودن، استثنا پرتاب می‌شود.
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید بررسی شود</param>
    /// <param name="equalityComparer">مقایسگری که برای بررسی خالی نبودن مقدار ورودی استفاده می‌شود</param>
    /// <param name="message">پیام خطا که در صورت خالی بودن مقدار ورودی، برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت خالی بودن مقدار ورودی</exception>
    public static void NotEmpty<T>(this Guard guard, T value, IEqualityComparer<T> equalityComparer, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (equalityComparer.Equals(value, default))
            throw new InvalidOperationException(message);
    }
}
