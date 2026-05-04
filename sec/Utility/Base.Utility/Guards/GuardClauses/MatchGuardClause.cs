using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;

public static class MatchGuardClause
{
    /// <summary>
    /// بررسی اینکه آیا مقدار ورودی با الگوی مشخص شده تطابق دارد و در صورت عدم تطابق، استثنا پرتاب می‌شود
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که باید با الگو مقایسه شود</param>
    /// <param name="pattern">الگوی مورد نظر برای تطابق با مقدار ورودی</param>
    /// <param name="message">پیام خطا که در صورت عدم تطابق مقدار ورودی با الگو برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم تطابق مقدار ورودی با الگوی مشخص شده</exception>
    public static void Match(this Guard guard, string value, string pattern, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (!Regex.IsMatch(value, pattern))
            throw new InvalidOperationException(message);
    }
}
