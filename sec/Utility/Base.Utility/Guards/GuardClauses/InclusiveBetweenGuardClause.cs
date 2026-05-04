using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;

public static class InclusiveBetweenGuardClause
{
    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی بین دو مقدار حداقل و حداکثر (شامل آن‌ها) است و در صورت عدم رعایت شرط، استثنا پرتاب می‌شود
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی برای بررسی</param>
    /// <param name="minimumValue">مقدار حداقل برای مقایسه</param>
    /// <param name="maximumValue">مقدار حداکثر برای مقایسه</param>
    /// <param name="comparer">مقایسه‌کننده برای بررسی مقادیر</param>
    /// <param name="message">پیام خطا که در صورت عدم رعایت شرط بین بودن برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم رعایت شرط بین بودن مقدار ورودی با مقدار حداقل و حداکثر</exception>
    public static void InclusiveBetween<T>(this Guard guard, T value, T minimumValue, T maximumValue, IComparer<T> comparer, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        int minimumValueComparerResult = comparer.Compare(value, minimumValue);
        int maximumValueComparerResult = comparer.Compare(value, maximumValue);

        if (minimumValueComparerResult == -1)
            throw new InvalidOperationException(message);

        if (maximumValueComparerResult == 1)
            throw new InvalidOperationException(message);
    }

    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی بین دو مقدار حداقل و حداکثر (شامل آن‌ها) است با استفاده از مقایسه‌گر پیش‌فرض و پرتاب استثنا در صورت عدم رعایت شرط
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی برای بررسی</param>
    /// <param name="minimumValue">مقدار حداقل برای مقایسه</param>
    /// <param name="maximumValue">مقدار حداکثر برای مقایسه</param>
    /// <param name="message">پیام خطا که در صورت عدم رعایت شرط بین بودن برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم رعایت شرط بین بودن مقدار ورودی با مقدار حداقل و حداکثر</exception>
    public static void InclusiveBetween<T>(this Guard guard, T value, T minimumValue, T maximumValue, string message)
        where T : IComparable<T>, IComparable
    {
        guard.InclusiveBetween(value, minimumValue, maximumValue, Comparer<T>.Default, message);
    }
}
