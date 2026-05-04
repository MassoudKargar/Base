using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;

public static class LessThanGuardClause
{
    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی کمتر از مقدار حداکثری است و در صورت عدم رعایت شرط، استثنا پرتاب می‌شود
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید بررسی شود</param>
    /// <param name="maximumValue">مقدار حداکثری برای مقایسه</param>
    /// <param name="comparer">مقایسه‌کننده برای بررسی مقادیر</param>
    /// <param name="message">پیام خطا که در صورت عدم رعایت شرط کمتر بودن برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم رعایت شرط کمتر بودن مقدار ورودی از مقدار حداکثری</exception>
    public static void LessThan<T>(this Guard guard, T value, T maximumValue, IComparer<T> comparer, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        int comparerResult = comparer.Compare(value, maximumValue);

        if (comparerResult > -1)
            throw new InvalidOperationException(message);
    }

    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی کمتر از مقدار حداکثری است با استفاده از مقایسه‌گر پیش‌فرض و پرتاب استثنا در صورت عدم رعایت شرط
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید بررسی شود</param>
    /// <param name="maximumValue">مقدار حداکثری برای مقایسه</param>
    /// <param name="message">پیام خطا که در صورت عدم رعایت شرط کمتر بودن برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم رعایت شرط کمتر بودن مقدار ورودی از مقدار حداکثری</exception>
    public static void LessThan<T>(this Guard guard, T value, T maximumValue, string message)
        where T : IComparable<T>, IComparable
    {
        guard.LessThan(value, maximumValue, Comparer<T>.Default, message);
    }
}
