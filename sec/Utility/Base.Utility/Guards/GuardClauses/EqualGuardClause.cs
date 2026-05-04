using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;
public static class EqualGuardClause
{
    /// <summary>
    /// بررسی برابری دو مقدار و پرتاب استثنا در صورت عدم برابری
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی برای بررسی</param>
    /// <param name="targetValue">مقدار هدف برای مقایسه</param>
    /// <param name="message">پیام خطا که در صورت عدم برابری برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم برابری مقدار ورودی با مقدار هدف</exception>
    public static void Equal<T>(this Guard guard, T value, T targetValue, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (!Equals(value, targetValue))
            throw new InvalidOperationException(message);
    }

    /// <summary>
    /// بررسی برابری دو مقدار با استفاده از مقایسه‌کننده سفارشی و پرتاب استثنا در صورت عدم برابری
    /// </summary>
    /// <typeparam name="T">نوع داده‌ای که باید بررسی شود</typeparam>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی برای بررسی</param>
    /// <param name="targetValue">مقدار هدف برای مقایسه</param>
    /// <param name="equalityComparer">مقایسه‌کننده برای بررسی برابری</param>
    /// <param name="message">پیام خطا که در صورت عدم برابری برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم برابری مقدار ورودی با مقدار هدف</exception>
    public static void Equal<T>(this Guard guard, T value, T targetValue, IEqualityComparer<T> equalityComparer, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (!equalityComparer.Equals(value, targetValue))
            throw new InvalidOperationException(message);
    }
}
