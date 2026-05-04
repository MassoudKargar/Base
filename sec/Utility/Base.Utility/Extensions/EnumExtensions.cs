namespace Base.Utility.Extensions;
public static class EnumExtensions
{
    /// <summary>
    /// برای دریافت توضیحات یک ویژگی از enum اگر [Description] داشته باشد از این متد استفاده می‌شود.
    /// </summary>
    /// <param name="enumValue">مقداری که قرار است توضحیات آن دریافت شود</param>
    /// <returns>متن داخل [Description] در صورتی که وجود داشته باشد و در غیراین صورت عنوان enums ارسال شده</returns>
    public static string GetEnumDescription(this Enum enumValue)
    {
        var memberInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (memberInfo is null) return string.Empty;
        var attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length is 0) return enumValue.ToString();
        var description = ((DescriptionAttribute)attributes.First(f => f.GetType() == typeof(DescriptionAttribute))).Description;
        return description;
    }
}