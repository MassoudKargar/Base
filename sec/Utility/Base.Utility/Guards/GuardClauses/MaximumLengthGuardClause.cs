using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;

public static class MaximumLengthGuardClause
{
    /// <summary>
    /// بررسی اینکه آیا طول مقدار ورودی بیشتر از حداکثر طول مجاز است و در صورت رعایت نکردن شرط، استثنا پرتاب می‌شود
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مقدار ورودی که طول آن باید بررسی شود</param>
    /// <param name="maximumLength">حداکثر طول مجاز برای مقدار ورودی</param>
    /// <param name="message">پیام خطا که در صورت رعایت نکردن شرط حداکثر طول، برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت بیشتر بودن طول مقدار ورودی از حداکثر طول مجاز</exception>
    public static void MaximumLength(this Guard guard, string value, int maximumLength, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value.Length > maximumLength)
            throw new InvalidOperationException(message);
    }

    /// <summary>
    /// بررسی اینکه آیا تعداد اعضای مجموعه ورودی بیشتر از حداکثر تعداد مجاز است و در صورت رعایت نکردن شرط، استثنا پرتاب می‌شود
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مجموعه ورودی که تعداد اعضای آن باید بررسی شود</param>
    /// <param name="maximumLength">حداکثر تعداد اعضای مجاز برای مجموعه ورودی</param>
    /// <param name="message">پیام خطا که در صورت رعایت نکردن شرط حداکثر تعداد اعضای مجموعه، برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت بیشتر بودن تعداد اعضای مجموعه از حداکثر تعداد مجاز</exception>
    public static void MaximumLength(this Guard guard, ICollection value, int maximumLength, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value.Count > maximumLength)
            throw new InvalidOperationException(message);
    }
}
