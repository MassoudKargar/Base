namespace Base.Utility.Extensions;

/// <summary>
/// کلاس افزونه برای بررسی مقدار GUID و تعیین خالی یا نال بودن آن.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// بررسی می‌کند که آیا یک GUID نال یا خالی است.
    /// </summary>
    /// <param name="guid">مقدار GUID نال‌پذیر.</param>
    /// <returns>اگر GUID نال یا خالی باشد، مقدار true برمی‌گرداند، در غیر این صورت false.</returns>
    public static bool IsNullOrEmpty(this Guid? guid) => guid is null or null;

    /// <summary>
    /// بررسی می‌کند که آیا یک GUID مقدار پیش‌فرض یا خالی است.
    /// </summary>
    /// <param name="guid">مقدار GUID.</param>
    /// <returns>اگر GUID مقدار پیش‌فرض باشد (00000000-0000-0000-0000-000000000000)، مقدار true برمی‌گرداند، در غیر این صورت false.</returns>
    public static bool IsEmpty(this Guid guid) => guid == default;
}
