namespace Base.Core.Domains.Exceptions;
/// <summary>
/// خطاهای لایه دامنه مرتبط با موجودیت‌ها (Entities) و اشیاء مقادیر (ValueObjects) به لایه‌های بالاتر از طریق یک Extension ارسال می‌شوند.
/// از آنجایی که هم موجودیت‌ها و هم اشیاء مقادیر خطا را به یک روش مشابه ارسال می‌کنند، یک کلاس Exception طراحی و پیاده‌سازی شده است.
/// برای تشخیص تفاوت بین خطا و محل وقوع آن در لایه‌های بالاتر، الگوی MicroType استفاده شده است.
/// </summary>
public abstract class DomainStateException(string message, params string[] parameters) : Exception(message)
{
    /// <summary>
    /// لیست پارامترهای خطا
    /// در صورتی که پارامتری وجود داشته باشد، پیام به عنوان یک الگو ارسال می‌شود و مقادیر پارامترها در جایگاه‌های خاصی در الگو قرار می‌گیرند.
    /// </summary>
    public string[] Parameters { get; } = parameters;

    public override string ToString()
    {
        if (Parameters?.Length < 1)
        {
            return Message;
        }

        var result = Message;

        if (Parameters == null) return result;
        for (var i = Parameters.Length - 1; i >= 0; i--)
        {
            var placeHolder = $"{{{i}}}";
            result = result.Replace(placeHolder, Parameters[i]);
        }
        return result;
    }
}
