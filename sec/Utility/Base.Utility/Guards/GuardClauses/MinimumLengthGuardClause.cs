using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;

public static class MinimumLengthGuardClause
{
    /// <summary>
    /// بررسی اینکه آیا طول مقدار ورودی کمتر از حداقل طول مجاز است و در صورت رعایت نکردن شرط، استثنا پرتاب می‌شود
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که طول آن باید بررسی شود</param>
    /// <param name="minimumLength">حداقل طول مجاز برای مقدار ورودی</param>
    /// <param name="message">پیام خطا که در صورت رعایت نکردن شرط حداقل طول، برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت کمتر بودن طول مقدار ورودی از حداقل طول مجاز</exception>
    public static void MinimumLength(this Guard guard, string value, int minimumLength, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value.Length < minimumLength)
            throw new InvalidOperationException(message);
    }

    /// <summary>
    /// بررسی اینکه آیا تعداد اعضای مجموعه ورودی کمتر از حداقل تعداد مجاز است و در صورت رعایت نکردن شرط، استثنا پرتاب می‌شود
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مجموعه ورودی که تعداد اعضای آن باید بررسی شود</param>
    /// <param name="minimumLength">حداقل تعداد اعضای مجاز برای مجموعه ورودی</param>
    /// <param name="message">پیام خطا که در صورت رعایت نکردن شرط حداقل تعداد اعضای مجموعه، برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت کمتر بودن تعداد اعضای مجموعه از حداقل تعداد مجاز</exception>
    public static void MinimumLength<T>(this Guard guard, ICollection value, int minimumLength, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value.Count < minimumLength)
            throw new InvalidOperationException(message);
    }
}
