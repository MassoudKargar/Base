using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;
public static class LessThanOrEqualGuardClause
{
    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی کمتر از یا برابر با مقدار حداقل است و در صورت عدم رعایت شرط، استثنا پرتاب می‌شود
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید بررسی شود</param>
    /// <param name="minimumValue">مقدار حداقل برای مقایسه</param>
    /// <param name="comparer">مقایسه‌کننده برای بررسی مقادیر</param>
    /// <param name="message">پیام خطا که در صورت عدم رعایت شرط کمتر از یا برابر بودن مقدار ورودی از مقدار حداقل برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم رعایت شرط کمتر از یا برابر بودن مقدار ورودی از مقدار حداقل</exception>
    public static void LessThanOrEqual<T>(this Guard guard, T value, T minimumValue, IComparer<T> comparer, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        int comparerResult = comparer.Compare(value, minimumValue);

        if (comparerResult > 0)
            throw new InvalidOperationException(message);
    }

    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی کمتر از یا برابر با مقدار حداقل است با استفاده از مقایسه‌گر پیش‌فرض و پرتاب استثنا در صورت عدم رعایت شرط
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید بررسی شود</param>
    /// <param name="minimumValue">مقدار حداقل برای مقایسه</param>
    /// <param name="message">پیام خطا که در صورت عدم رعایت شرط کمتر از یا برابر بودن مقدار ورودی از مقدار حداقل برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم رعایت شرط کمتر از یا برابر بودن مقدار ورودی از مقدار حداقل</exception>
    public static void LessThanOrEqual<T>(this Guard guard, T value, T minimumValue, string message)
        where T : IComparable<T>, IComparable
    {
        guard.LessThanOrEqual(value, minimumValue, Comparer<T>.Default, message);
    }
}
