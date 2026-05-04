namespace Base.Utility.Extensions;

/// <summary>
/// کلاس افزونه برای تبدیل تاریخ و زمان به قالب‌های یونیکس (Unix Time) و برعکس، با پشتیبانی از منطقه زمانی (TimeZone).
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// تاریخ و زمان را به زمان یونیکس (بر حسب میلی‌ثانیه) در منطقه زمانی مشخص تبدیل می‌کند.
    /// </summary>
    /// <param name="dateTime">تاریخ و زمان مبدا.</param>
    /// <param name="timeZoneInfo">منطقه زمانی مقصد.</param>
    /// <returns>زمان یونیکس به میلی‌ثانیه.</returns>
    public static long ToUnixTimeMillisecond(this DateTime dateTime, TimeZoneInfo timeZoneInfo)
        => TimeZoneInfo.ConvertTime(new DateTimeOffset(dateTime), timeZoneInfo).ToUnixTimeMilliseconds();

    /// <summary>
    /// تاریخ و زمان نال‌پذیر را به زمان یونیکس (بر حسب میلی‌ثانیه) در منطقه زمانی مشخص تبدیل می‌کند.
    /// </summary>
    /// <param name="dateTime">تاریخ و زمان مبدا (Nullable).</param>
    /// <param name="timeZoneInfo">منطقه زمانی مقصد.</param>
    /// <returns>زمان یونیکس به میلی‌ثانیه یا نال.</returns>
    public static long? ToUnixTimeMillisecond(this DateTime? dateTime, TimeZoneInfo timeZoneInfo)
        => dateTime is not null ? TimeZoneInfo.ConvertTime(new DateTimeOffset(dateTime.Value), timeZoneInfo).ToUnixTimeMilliseconds() : null;

    /// <summary>
    /// زمان یونیکس (بر حسب میلی‌ثانیه) را به تاریخ و زمان در منطقه زمانی مشخص تبدیل می‌کند.
    /// </summary>
    /// <param name="unixTimeMilliseconds">زمان یونیکس بر حسب میلی‌ثانیه.</param>
    /// <param name="timeZoneInfo">منطقه زمانی مقصد.</param>
    /// <returns>تاریخ و زمان مقصد.</returns>
    public static DateTime ToDateTime(this long unixTimeMilliseconds, TimeZoneInfo timeZoneInfo)
        => TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds), timeZoneInfo).DateTime;

    /// <summary>
    /// زمان یونیکس نال‌پذیر (بر حسب میلی‌ثانیه) را به تاریخ و زمان در منطقه زمانی مشخص تبدیل می‌کند.
    /// </summary>
    /// <param name="unixTimeMilliseconds">زمان یونیکس بر حسب میلی‌ثانیه (Nullable).</param>
    /// <param name="timeZoneInfo">منطقه زمانی مقصد.</param>
    /// <returns>تاریخ و زمان مقصد یا نال.</returns>
    public static DateTime? ToDateTime(this long? unixTimeMilliseconds, TimeZoneInfo timeZoneInfo)
        => unixTimeMilliseconds is not null ? TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds.Value), timeZoneInfo).DateTime : null;

    /// <summary>
    /// تاریخ و زمان را به زمان یونیکس محلی (بر حسب میلی‌ثانیه) تبدیل می‌کند.
    /// </summary>
    public static long ToLocalUnixTimeMillisecond(this DateTime dateTime) => dateTime.ToUnixTimeMillisecond(TimeZoneInfo.Local);

    /// <summary>
    /// تاریخ و زمان نال‌پذیر را به زمان یونیکس محلی (بر حسب میلی‌ثانیه) تبدیل می‌کند.
    /// </summary>
    public static long? ToLocalUnixTimeMillisecond(this DateTime? dateTime) => dateTime.ToUnixTimeMillisecond(TimeZoneInfo.Local);

    /// <summary>
    /// زمان یونیکس محلی (بر حسب میلی‌ثانیه) را به تاریخ و زمان محلی تبدیل می‌کند.
    /// </summary>
    public static DateTime ToLocalDateTime(this long unixTimeMilliseconds) => unixTimeMilliseconds.ToDateTime(TimeZoneInfo.Local);

    /// <summary>
    /// زمان یونیکس نال‌پذیر محلی (بر حسب میلی‌ثانیه) را به تاریخ و زمان محلی تبدیل می‌کند.
    /// </summary>
    public static DateTime? ToLocalDateTime(this long? unixTimeMilliseconds) => unixTimeMilliseconds.ToDateTime(TimeZoneInfo.Local);

    /// <summary>
    /// تاریخ و زمان را به زمان یونیکس UTC (بر حسب میلی‌ثانیه) تبدیل می‌کند.
    /// </summary>
    public static long ToUtcUnixTimeMillisecond(this DateTime dateTime) => dateTime.ToUnixTimeMillisecond(TimeZoneInfo.Utc);

    /// <summary>
    /// تاریخ و زمان نال‌پذیر را به زمان یونیکس UTC (بر حسب میلی‌ثانیه) تبدیل می‌کند.
    /// </summary>
    public static long? ToUtcUnixTimeMillisecond(this DateTime? dateTime) => dateTime.ToUnixTimeMillisecond(TimeZoneInfo.Utc);

    /// <summary>
    /// زمان یونیکس UTC (بر حسب میلی‌ثانیه) را به تاریخ و زمان UTC تبدیل می‌کند.
    /// </summary>
    public static DateTime ToUtcDateTime(this long unixTimeMilliseconds) => unixTimeMilliseconds.ToDateTime(TimeZoneInfo.Utc);

    /// <summary>
    /// زمان یونیکس نال‌پذیر UTC (بر حسب میلی‌ثانیه) را به تاریخ و زمان UTC تبدیل می‌کند.
    /// </summary>
    public static DateTime? ToUtcDateTime(this long? unixTimeMilliseconds) => unixTimeMilliseconds.ToDateTime(TimeZoneInfo.Utc);
}
