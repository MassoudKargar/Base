namespace Base.Utility.DateTimes;
/// <summary>
/// Helper methods to work with Gregorian date
/// </summary>
public static class DateTimeUtils
{
    /// <summary>
    /// زمان استاندارد ایران
    /// </summary>
    public static readonly TimeZoneInfo? IranStandardTime;

    /// <summary>
    /// عصر میلادی به صورت DateTime نمایش داده شده است
    /// </summary>
    public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    static DateTimeUtils()
    {
        IranStandardTime = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(timeZoneInfo =>
            timeZoneInfo.StandardName.Contains("Iran") ||
            timeZoneInfo.StandardName.Contains("Tehran") ||
            timeZoneInfo.Id.Contains("Iran") ||
            timeZoneInfo.Id.Contains("Tehran"));
        if (IranStandardTime == null)
        {
#if NET40 || NET45 || NET46
            throw new PlatformNotSupportedException($"این OS[{Environment.OSVersion.Platform}, {Environment.OSVersion.Version}] دستیار استاندارد زمان ایران را پشتporte نمی‌کند.");
#else
            throw new PlatformNotSupportedException($"این OS[{System.Runtime.InteropServices.RuntimeInformation.OSDescription}] دستیار استاندارد زمان ایران را پشتporte نمی‌کند.");
#endif
        }
    }



    /// <summary>
    /// محاسبه سن
    /// </summary>
    /// <param name="birthday">تاریخ تولد</param>
    /// <param name="comparisonBase">بنتهای مقایسه بر اساس حال حاضر</param>
    /// <param name="dateTimeOffsetPart">جزء که در این زمان استفاده شود؟</param>
    /// <returns>سن</returns>
    public static int GetAge(this DateTimeOffset birthday, DateTime comparisonBase, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return birthday.GetDateTimeOffsetPart(dateTimeOffsetPart).GetAge(comparisonBase);
    }

    /// <summary>
    /// محاسبه سن
    /// پایه برای محاسبه حال حاضر
    /// </summary>
    /// <param name="birthday">تاریخ تولد</param>
    /// <returns>سن</returns>
    public static int GetAge(this DateTimeOffset birthday)
    {
        var birthdayDateTime = birthday.UtcDateTime;
        var now = DateTime.UtcNow;
        return birthdayDateTime.GetAge(now);
    }

    /// <summary>
    /// محاسبه سن
    /// </summary>
    /// <param name="birthday">تاریخ تولد</param>
    /// <param name="comparisonBase">بنتهای مقایسه بر اساس حال حاضر</param>
    /// <returns>سن</returns>
    public static int GetAge(this DateTime birthday, DateTime comparisonBase)
    {
        var now = comparisonBase;
        var age = now.Year - birthday.Year;
        if (now < birthday.AddYears(age)) age--;

        return age;
    }



    /// <summary>
    /// محاسبه سن
    /// پایه برای محاسبه حال حاضر
    /// </summary>
    /// <param name="birthday">تاریخ تولد</param>
    /// <returns>سن</returns>
    public static int GetAge(this DateTime birthday)
    {
        var now = birthday.Kind.GetNow();
        return birthday.GetAge(now);
    }

    /// <summary>
    /// بخش زمانی خاص این مکان را دریافت کن
    /// </summary>
    public static DateTime GetDateTimeOffsetPart(
        this DateTimeOffset dateTimeOffset,
        DateTimeOffsetPart dataDateTimeOffsetPart)
    {
        switch (dataDateTimeOffsetPart)
        {
            case DateTimeOffsetPart.DateTime:
                return dateTimeOffset.DateTime;

            case DateTimeOffsetPart.LocalDateTime:
                return dateTimeOffset.LocalDateTime;

            case DateTimeOffsetPart.UtcDateTime:
                return dateTimeOffset.UtcDateTime;

            case DateTimeOffsetPart.IranLocalDateTime:
                return dateTimeOffset.ToIranTimeZoneDateTimeOffset().DateTime;

            default:
                throw new ArgumentOutOfRangeException(nameof(dataDateTimeOffsetPart), dataDateTimeOffsetPart, null);
        }
    }

    /// <summary>
    /// زمان فعلی بر اساس نوع زمان برگرداند
    /// </summary>
    /// <param name="dataDateTimeKind">نوع زمان ورودی</param>
    /// <returns>الآن</returns>
    public static DateTime GetNow(this DateTimeKind dataDateTimeKind)
    {
        switch (dataDateTimeKind)
        {
            case DateTimeKind.Utc:
                return DateTime.UtcNow;
            default:
                return DateTime.Now;
        }
    }

    /// <summary>
    /// تبدیل زمان منطقه محلی به زمان منطقه ایران
    /// </summary>
    public static DateTimeOffset ToIranTimeZoneDateTimeOffset(this DateTimeOffset dateTimeOffset)
    {
        return TimeZoneInfo.ConvertTime(dateTimeOffset, IranStandardTime);
    }

    /// <summary>
    /// تبدیل زمان منطقه محلی به زمان منطقه ایران
    /// </summary>
    public static DateTime ToIranTimeZoneDateTime(this DateTime dateTime)
    {
        return dateTime;
        //return TimeZoneInfo.ConvertTime(dateTime, IranStandardTime);
    }

    /// <summary>
    /// یک <see cref="DateTime"/> مورد نظر را به میلی ثانیه‌ها از زمان شروع (Epoch) تبدیل می‌کند.
    /// </summary>
    /// <param name="dateTime">یک <see cref="DateTime"/> مورد نظر</param>
    /// <returns>میلی ثانیه‌ها از زمان شروع (Epoch)</returns>
    public static long ToEpochMilliseconds(this DateTime dateTime)
    {
        return (long)dateTime.ToUniversalTime().Subtract(Epoch).TotalMilliseconds;
    }


    /// <summary>
    /// یک <see cref="DateTime"/> مورد نظر را به ثانیه‌ها از زمان شروع (Epoch) تبدیل می‌کند.
    /// </summary>
    /// <param name="dateTime">یک <see cref="DateTime"/> مورد نظر</param>
    /// <returns>تایم‌stamp سرور خنک‌مدت (Unix)</returns>
    public static long ToEpochSeconds(this DateTime dateTime)
    {
        return dateTime.ToEpochMilliseconds() / 1000;
    }


    /// <summary>
    /// بررسی می‌کند آیا تاریخ مورد نظر بین دو تاریخ ارائه شده قرار دارد
    /// </summary>
    public static bool IsBetween(this DateTime date, DateTime startDate, DateTime endDate, bool compareTime = false)
    {
        return compareTime ? date >= startDate && date <= endDate : date.Date >= startDate.Date && date.Date <= endDate.Date;
    }

    /// <summary>
    /// بررسی می‌کند آیا تاریخ مورد نظر آخر روز ماه است
    /// </summary>
    public static bool IsLastDayOfTheMonth(this DateTime dateTime)
    {
        return dateTime == new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
    }


    /// <summary>
    /// بررسی می‌کند آیا تاریخ مورد نظر هفته‌ای است (شنبه یا جمعه)
    /// </summary>
    public static bool IsWeekend(this DateTime value)
    {
        return value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday;
    }

    /// <summary>
    /// تعیین می‌کند سال مورد نظر یک سال قابل دستشدن (لپ‌عام) است یا خیر.
    /// </summary>
    public static bool IsLeapYear(this DateTime value)
    {
        return DateTime.DaysInMonth(value.Year, 2) == 29;
    }

    /// <summary>
    /// تبدیل یک DateTime به DateTimeOffset
    /// </summary>
    /// <param name="dt">منبع DateTime.</param>
    /// <param name="offset">اختلاف زمانی</param>
    public static DateTimeOffset ToDateTimeOffset(this DateTime dt, TimeSpan offset)
    {
        if (dt == DateTime.MinValue)
            return DateTimeOffset.MinValue;

        return new DateTimeOffset(dt.Ticks, offset);
    }

    /// <summary>
    /// تبدیل یک DateTime به DateTimeOffset
    /// </summary>
    /// <param name="dt">مقدار DateTime مبدأ.</param>
    /// <param name="offsetInHours">اختلاف زمانی به ساعت (به صورت عددی)</param>
    /// <returns>DateTimeOffset با اعمال اختلاف زمانی</returns>
    public static DateTimeOffset ToDateTimeOffset(this DateTime dt, double offsetInHours = 0)
        => dt.ToDateTimeOffset(offsetInHours == 0 ? TimeSpan.Zero : TimeSpan.FromHours(offsetInHours));

}
