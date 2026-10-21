using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;
public static class EmptyGuardClause
{
    /// <summary>
    /// بررسی خالی بودن مقدار ورودی و پرتاب استثنا در صورت نیاز
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید بررسی شود</param>
    /// <param name="message">پیام خطا که در صورت نیاز برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم مطابقت مقدار با شرایط خالی بودن</exception>
    public static void Empty<T>(this Guard guard, T value, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value == null)
            return;

        if (value is ICollection collectionValue && collectionValue.Count == 0)
            return;

        if (value is IEnumerable enumerableValue && !enumerableValue.GetEnumerator().MoveNext())
            return;

        if (value is string strValue && string.IsNullOrWhiteSpace(strValue))
            return;

        if (!Equals(value, default(T)))
            throw new InvalidOperationException(message);
    }

    /// <summary>
    /// بررسی خالی بودن مقدار ورودی با استفاده از مقایسه‌های سفارشی و پرتاب استثنا در صورت نیاز
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید بررسی شود</param>
    /// <param name="equalityComparer">مقایسه‌کننده برای بررسی مقدار پیش‌فرض</param>
    /// <param name="message">پیام خطا که در صورت نیاز برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم مطابقت مقدار با شرایط خالی بودن</exception>
    public static void Empty<T>(this Guard guard, T value, IEqualityComparer<T> equalityComparer, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (!equalityComparer.Equals(value, default))
            throw new InvalidOperationException(message);
    }
}
