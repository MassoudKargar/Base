namespace Base.Core.Application.Exceptions;

/// <summary>
/// برای مدیرست خطاهای منطقی برنامه و برگرداندن پیقام مناسب
/// </summary>
public class LogicException : AppException
{
    public LogicException()
        : base(ApiResultStatusCode.Conflict)
    {
    }

    public LogicException(string message)
        : base(ApiResultStatusCode.Conflict, message)
    {
    }

    public LogicException(object additionalData)
        : base(ApiResultStatusCode.Conflict, additionalData)
    {
    }

    public LogicException(string message, object additionalData)
        : base(ApiResultStatusCode.Conflict, message, additionalData)
    {
    }

    public LogicException(string message, Exception exception)
        : base(ApiResultStatusCode.Conflict, message, exception)
    {
    }

    public LogicException(string message, Exception exception, object additionalData)
        : base(ApiResultStatusCode.Conflict, message, exception, additionalData)
    {
    }
}