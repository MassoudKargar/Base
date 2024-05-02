namespace Base.Utility.DateTimes;
/// <summary>
/// DateTimeOffset Part
/// The source of this section was originally taken from the works of Mr. Vahid Nasiri. After completion and final corrections, addressing etc. will be corrected
/// </summary>
public enum DateTimeOffsetPart
{
    /// <summary>
    /// The time field returns the value regardless of the offset and will not be converted to the server's local time
    /// </summary>
    DateTime,

    /// <summary>
    /// Returns the time field according to the time zone of the server on which the application is running
    /// </summary>
    LocalDateTime,

    /// <summary>
    /// The Coordinated Universal Time (UTC) date and time of the current System.DateTimeOffset
    /// </summary>
    UtcDateTime,

    /// <summary>
    /// Converts this location to Iran's time zone and returns the value
    /// </summary>
    IranLocalDateTime
}
