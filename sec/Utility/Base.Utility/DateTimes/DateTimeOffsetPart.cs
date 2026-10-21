namespace Base.Utility.DateTimes;
/// <summary>
/// جزء DateTimeOffset
/// اصل این بخش از کارهای آقا Vahid Nasiri به طور اصلی برداشته شده است. پس از تمام‌کردن و اصلاحات نهایی، مراجعه و غیره را اصلاح خواهد کرد
/// </summary>
public enum DateTimeOffsetPart
{
    /// <summary>
    /// مقدار زمان با توجه به اختلاف زمانی، برمی‌گردد و به زمان محلی سرور نمایش داده نخواهد شد
    /// </summary>
    DateTime,

    /// <summary>
    /// زمان مورد نظر بر اساس زمان منطقه سروری که برنامه در آن قابل اجرا است، برمی‌گرداند
    /// </summary>
    LocalDateTime,

    /// <summary>
    /// زمان UTC فعلی System.DateTimeOffset
    /// </summary>
    UtcDateTime,

    /// <summary>
    /// این موقعیت را به منطقه زمان ایران تبدیل می‌کند و مقدار آن را برمی‌گرداند
    /// </summary>
    IranLocalDateTime
}
