using Base.Utility.Guards;

namespace Base.Utility.Guards.GuardClauses;

public static class LengthGuardClause
{
    /// <summary>
    /// بررسی اینکه آیا طول رشته ورودی برابر با طول مشخص شده است و در صورت عدم رعایت شرط، استثنا پرتاب می‌شود
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">رشته‌ای که طول آن باید بررسی شود</param>
    /// <param name="length">طول مورد نظر برای مقایسه</param>
    /// <param name="message">پیام خطا که در صورت عدم رعایت شرط طول برابر با مقدار مشخص شده برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم رعایت شرط برابر بودن طول رشته با مقدار مشخص شده</exception>
    public static void Length(this Guard guard, string value, int length, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value.Length != length)
            throw new InvalidOperationException(message);
    }

    /// <summary>
    /// بررسی اینکه آیا تعداد آیتم‌های مجموعه ورودی برابر با تعداد مشخص شده است و در صورت عدم رعایت شرط، استثنا پرتاب می‌شود
    /// </summary>
    /// <param name="guard">نمونه‌ای از کلاس Guard برای اجرای گاردها</param>
    /// <param name="value">مجموعه‌ای که تعداد آن باید بررسی شود</param>
    /// <param name="length">تعداد مورد نظر برای مقایسه</param>
    /// <param name="message">پیام خطا که در صورت عدم رعایت شرط برابر بودن تعداد آیتم‌ها با مقدار مشخص شده برای پرتاب استثنا استفاده می‌شود</param>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن پیام</exception>
    /// <exception cref="InvalidOperationException">در صورت عدم رعایت شرط برابر بودن تعداد آیتم‌ها با مقدار مشخص شده</exception>
    public static void Length(this Guard guard, ICollection value, int length, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value.Count != length)
            throw new InvalidOperationException(message);
    }
}
