namespace Base.Core.Domains.Exceptions;
/// <summary>
/// خطاهای مربوط به وضعیت نادرست در موجودیت‌ها توسط این کلاس ارسال می‌شود.
/// </summary>
/// <param name="message">پیام خطا یا الگو</param>
/// <param name="parameters">پارامترهایی که در صورت وجود در الگوی پیام قرار می‌گیرند</param>
public class InvalidEntityStateException(string message, params string[] parameters)
    : DomainStateException(message, parameters);
