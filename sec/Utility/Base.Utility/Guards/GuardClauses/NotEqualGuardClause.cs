using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;
public static class NotEqualGuardClause
{
    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی با مقدار هدف برابر نیست و در صورت برابر بودن، استثنا پرتاب می‌شود.
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید بررسی شود</param>
    /// <param name="targetValue">مقدار هدف که با مقدار ورودی مقایسه می‌شود</param>
    /// <param name="message">پیام خطا که در صورت برابر بودن مقادیر، برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت برابر بودن مقدار ورودی با مقدار هدف</exception>
    public static void NotEqual<T>(this Guard guard, T value, T targetValue, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (Equals(value, targetValue))
            throw new InvalidOperationException(message);
    }
}
