namespace Base.Utility.Extensions;

/// <summary>
/// کلاس افزونه برای کار با رشته‌ها.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// کاراکتر "ی" عربی.
    /// </summary>
    public const char ArabicYeChar = (char)1610;

    /// <summary>
    /// کاراکتر "ی" فارسی.
    /// </summary>
    public const char PersianYeChar = (char)1740;

    /// <summary>
    /// کاراکتر "ک" عربی.
    /// </summary>
    public const char ArabicKeChar = (char)1603;

    /// <summary>
    /// کاراکتر "ک" فارسی.
    /// </summary>
    public const char PersianKeChar = (char)1705;

    /// <summary>
    /// جایگزینی "ی" عربی با "ی" فارسی و "ک" عربی با "ک" فارسی.
    /// </summary>
    /// <param name="data">رشته ورودی.</param>
    /// <returns>رشته تصحیح‌شده.</returns>
    public static string ApplyCorrectYeKe(this object data) =>
        data == null ? null : data.ToString().ApplyCorrectYeKe();

    /// <summary>
    /// نسخه رشته‌ای `ApplyCorrectYeKe`.
    /// </summary>
    /// <param name="data">رشته ورودی.</param>
    /// <returns>رشته تصحیح‌شده.</returns>
    public static string ApplyCorrectYeKe(this string data) =>
        string.IsNullOrWhiteSpace(data) ?
        string.Empty :
        data.Replace(ArabicYeChar, PersianYeChar).Replace(ArabicKeChar, PersianKeChar).Trim();

    /// <summary>
    /// تبدیل رشته به عدد صحیح طولانی (`long`) با جایگزین پیش‌فرض در صورت عدم موفقیت.
    /// </summary>
    /// <param name="input">رشته ورودی.</param>
    /// <param name="replacement">مقدار جایگزین در صورت خطا.</param>
    /// <returns>مقدار عددی یا مقدار جایگزین.</returns>
    public static long ToSafeLong(this string input, long replacement = long.MinValue) =>
        long.TryParse(input, out long result) ? result : replacement;

    /// <summary>
    /// تبدیل رشته به `long` قابل نال.
    /// </summary>
    /// <param name="input">رشته ورودی.</param>
    /// <returns>مقدار عددی یا نال.</returns>
    public static long? ToSafeNullableLong(this string input) =>
        long.TryParse(input, out long result) ? result : null;

    /// <summary>
    /// تبدیل رشته به `int` با جایگزین پیش‌فرض در صورت عدم موفقیت.
    /// </summary>
    /// <param name="input">رشته ورودی.</param>
    /// <param name="replacement">مقدار جایگزین در صورت خطا.</param>
    /// <returns>مقدار عددی یا مقدار جایگزین.</returns>
    public static int ToSafeInt(this string input, int replacement = int.MinValue) =>
        int.TryParse(input, out int result) ? result : replacement;

    /// <summary>
    /// تبدیل رشته به `int` قابل نال.
    /// </summary>
    /// <param name="input">رشته ورودی.</param>
    /// <returns>مقدار عددی یا نال.</returns>
    public static int? ToSafeNullableInt(this string input) =>
        int.TryParse(input, out int result) ? result : null;

    /// <summary>
    /// تبدیل یک رشته یا نال به رشته خالی در صورت نال بودن.
    /// </summary>
    /// <param name="input">رشته ورودی.</param>
    /// <returns>رشته یا مقدار خالی.</returns>
    public static string ToStringOrEmpty(this string? input) => input ?? string.Empty;

    /// <summary>
    /// تبدیل رشته به فرمت `underscore_case`.
    /// </summary>
    /// <param name="input">رشته ورودی.</param>
    /// <returns>رشته در قالب `underscore_case`.</returns>
    public static string ToUnderscoreCase(this string input) =>
        string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();

    /// <summary>
    /// تبدیل رشته به آرایه بایت با استفاده از کدگذاری UTF-8.
    /// </summary>
    /// <param name="input">رشته ورودی.</param>
    /// <returns>آرایه بایت معادل.</returns>
    public static byte[] ToByteArray(this string input) =>
        System.Text.Encoding.UTF8.GetBytes(input);

    /// <summary>
    /// تبدیل آرایه بایت به رشته با استفاده از کدگذاری UTF-8.
    /// </summary>
    /// <param name="input">آرایه بایت ورودی.</param>
    /// <returns>رشته معادل.</returns>
    public static string FromByteArray(this byte[] input) =>
        System.Text.Encoding.UTF8.GetString(input);
}
